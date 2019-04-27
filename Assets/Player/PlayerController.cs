using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0;

    public float maxSpeed = 10;

    public State state;

    public float turnSpeed;

    public float acceleration;

    public float deacceleration;

    private Plane positionPlane = new Plane();

    private Vector3 aimVector;

    private Vector3 actualDirection = new Vector3(1,0,0);

    private Rigidbody rbody;

    [SerializeField]
    private Camera playerCamera;

    public enum State
    {
        Normal,
        Spinning,
        Dashing
    }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameContext.isGamePaused) { return; }
        
        // Set velocity.
        rbody.velocity = actualDirection * moveSpeed;

        positionPlane.SetNormalAndPosition(Vector3.up, transform.position); // Update position plane, normal can be changed in case of slopes in the future.
        Vector3 aimVector = GetAimVector();

        if (aimVector != Vector3.zero)
        {
            actualDirection = Vector3.RotateTowards(actualDirection, aimVector, turnSpeed, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(actualDirection, Vector3.up), 1f);
        }

        // Increase or decrease speed.
        switch (state)
        {
            case State.Normal:
            {
                moveSpeed = Mathf.Min(moveSpeed + acceleration * Time.deltaTime, maxSpeed);
                break;
            }
            case State.Spinning:
            {
                moveSpeed += deacceleration * Time.deltaTime;
                break;
            }
        }
    }

    /// <summary>
    /// Gets the direction the player is attempting to move in.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetAimVector()
    {
        if (Input.touchCount > 0)
        {
            Ray r = playerCamera.ScreenPointToRay(Input.GetTouch(0).position);
            positionPlane.Raycast(r, out float distanceToPoint);
            aimVector = r.GetPoint(distanceToPoint) - transform.position;
            return aimVector.normalized;
        }
        else if (Application.isEditor && Input.GetMouseButton(0)) // For testing mobile input in editor.
        {
            Ray r = playerCamera.ScreenPointToRay(Input.mousePosition);
            positionPlane.Raycast(r, out float distanceToPoint);
            aimVector = r.GetPoint(distanceToPoint) - transform.position;
            return aimVector.normalized;
        }
        else
        {
            return Vector3.zero; // Used instead of null.
        }
    }

}

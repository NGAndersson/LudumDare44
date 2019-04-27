using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public State state;

    private Plane positionPlane = new Plane();

    private Vector3 aimVector;

    private Vector3 actualDirection;

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
        if (Input.GetButton("Fire1") || Input.touches.Length > 0)
        {
            rbody.AddForce(transform.forward * 10, ForceMode.Acceleration);
        }

        positionPlane.SetNormalAndPosition(Vector3.up, transform.position); // Update position plane, normal can be changed in case of slopes in the future.
        Vector3 aimVector = GetAimVector();
        
        if (aimVector != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimVector, Vector3.up), 1f);
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

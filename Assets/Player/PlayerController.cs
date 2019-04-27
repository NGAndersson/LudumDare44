using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.0f;
    public float maxSpeed = 10.0f;
    public float minSpeed = 4.0f;

    public float turnSpeed = 6.0f;
    public float spinsPerSecond = 3.0f;

    public float acceleration = 1.0f;
    public float deacceleration = 2.0f;

    public State state;
    public Transform visuals;

    private Plane positionPlane = new Plane();
    public Plane PositionPlane
    {
        get
        {
            return positionPlane;
        }
    }

    private Vector3 aimVector;

    private Vector3 actualDirection = new Vector3(1, 0, 0);

    private Rigidbody rbody;

    [SerializeField]
    private ChargeManager chargeManager = null;

    [SerializeField]
    private Camera playerCamera = null;

    public Camera Camera
    {
        get
        {
            return playerCamera;
        }
    }

    public enum State
    {
        Normal,
        Spinning,
        Dashing
    }

    Vector3 originPosition;
    Quaternion originRotation;

    // Start is called before the first frame update
    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;

        rbody = GetComponent<Rigidbody>();
        moveSpeed = minSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameContext.isGamePaused) { return; }

        // Set velocity.
        rbody.velocity = actualDirection * moveSpeed;
        visuals.GetComponent<Animator>().SetFloat("MovementSpeed", (moveSpeed - minSpeed) / (maxSpeed - minSpeed));

        positionPlane.SetNormalAndPosition(Vector3.up, transform.position); // Update position plane, normal can be changed in case of slopes in the future.
        Vector3 aimVector = GetAimVector();

        if (aimVector != Vector3.zero)
        {
            actualDirection = Vector3.RotateTowards(actualDirection, aimVector, turnSpeed * Time.deltaTime, 0.0f);
        }

        // Rotate visuals differently depending on state.
        switch (state)
        {
            case State.Normal:
                {
                    visuals.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(actualDirection, Vector3.up), 1f);
                    break;
                }
            case State.Spinning:
                {
                    visuals.rotation = Quaternion.Euler(visuals.rotation.eulerAngles.x, visuals.rotation.eulerAngles.y + spinsPerSecond * 360 * Time.deltaTime, visuals.rotation.eulerAngles.z);
                    break;
                }
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
                    if (moveSpeed <= minSpeed)
                        SetState(State.Normal);
                    break;
                }
        }
    }

    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.Normal:
                {
                    state = State.Normal;
                    visuals.GetComponent<Animator>().SetTrigger("Skate");
                    turnSpeed = 6.0f;
                    acceleration = 1.0f;
                    deacceleration = -2.0f;
                    break;
                }
            case State.Spinning:
                {
                    state = State.Spinning;
                    chargeManager.ActivateSpin();
                    visuals.GetComponent<Animator>().SetTrigger("Spin");
                    turnSpeed = 3.0f;
                    acceleration = 1.0f;
                    deacceleration = -2.0f;
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

    private void Die(Vector3 deathVector)
    {

    }

    public void Reset()
    {
        transform.position = originPosition;
        moveSpeed = 0.0f;
        maxSpeed = 10.0f;
        minSpeed = 4.0f;
        turnSpeed = 6.0f;
        spinsPerSecond = 3.0f;
        acceleration = 1.0f;
        deacceleration = -2.0f;
        actualDirection = new Vector3(1f, 0f, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            switch (state)
            {
                case State.Normal:
                    {
                        Die(collision.transform.position - transform.position);
                        break;
                    }
                case State.Spinning:
                    {
                        collision.gameObject.GetComponent<Enemy>().Die(transform.position - collision.transform.position);
                        break;
                    }
            }
        }
    }
}

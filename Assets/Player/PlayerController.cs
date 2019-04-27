using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.0f;
    [System.NonSerialized]
    public float maxSpeed = 18f;
    [System.NonSerialized]
    public float minSpeed = 8f;

    public float turnSpeed = 6.0f;
    public float spinsPerSecond = 3.0f;

    private float acceleration = 2.5f;
    private float deacceleration = -2.0f;

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

    private MenuEvents menu;

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

    bool alreadyIncludedCollision = false;
    const float TotalCollisionTimeThreshold = 0.7f;
    float totalCollisionTime = 0f;
    const float CollisionFreeTimeThreshold = 0.5f;
    float collisionFreeTime = 0f;

    public enum State
    {
        Normal,
        Spinning,
        Dashing
    }

    Vector3 originPosition;
    Quaternion originRotation;
    new Camera camera;
    CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        menu = Utilities.Scene.findExactlyOne<MenuEvents>();
        originPosition = transform.position;
        originRotation = transform.rotation;

        rbody = GetComponent<Rigidbody>();
        moveSpeed = minSpeed;
        camera = Camera.main;
        cameraShake = camera.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameContext.isGamePaused) { return; }

        // Spin debug
        if (Input.GetKeyDown(KeyCode.S))
            SetState(State.Spinning);

        // Set velocity.
        rbody.velocity = actualDirection * moveSpeed;
        visuals.GetComponent<Animator>().SetFloat("MovementSpeed", 1.5f - (moveSpeed - minSpeed) / (maxSpeed - minSpeed));

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
                    float speedPercentage = 1 - (moveSpeed / maxSpeed);
                    float currAcceleration = acceleration * speedPercentage;

                    moveSpeed = Mathf.Min(moveSpeed + currAcceleration * Time.deltaTime, maxSpeed);
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
                    //acceleration = 1.0f;
                    //deacceleration = -2.0f;
                    break;
                }
            case State.Spinning:
                {
                    state = State.Spinning;
                    chargeManager.ActivateSpin();
                    visuals.GetComponent<Animator>().SetTrigger("Spin");
                    turnSpeed = 3.0f;
                    //acceleration = 1.0f;
                    //deacceleration = -2.0f;
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
        menu.ToggleMenuShow();
    }

    public void Reset()
    {
        transform.position = originPosition;
        //maxSpeed = 10.0f;
        //minSpeed = 4.0f;
        moveSpeed = minSpeed;
        turnSpeed = 6.0f;
        spinsPerSecond = 3.0f;
        //acceleration = 1.0f;
        //deacceleration = -2.0f;
        actualDirection = new Vector3(1f, 0f, 0f);
        totalCollisionTime = 0;
        collisionFreeTime = 0;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            switch (state)
            {
                case State.Spinning:
                    {
                        collision.gameObject.GetComponent<Enemy>().Die(transform.position - collision.transform.position);
                        break;
                    }
                default:
                    break;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            switch (state)
            {
                case State.Normal:
                    {
                        if (alreadyIncludedCollision == false)
                        {
                            collisionFreeTime = 0;
                            alreadyIncludedCollision = true;
                            totalCollisionTime += Time.deltaTime;
                            cameraShake.shakeDuration = 0.1f;
                            cameraShake.shakeAmount = totalCollisionTime;
                            if (totalCollisionTime >= TotalCollisionTimeThreshold)
                            {
                                Die(collision.transform.position - transform.position);
                                cameraShake.shakeDuration = 0;
                            }
                        }
                        break;
                    }
                case State.Spinning:
                    {
                        collision.gameObject.GetComponent<Enemy>().Die(transform.position - collision.transform.position);
                        break;
                    }
                default: break;
            }
        }
    }

    private void LateUpdate()
    {
        if(alreadyIncludedCollision)
        {
            collisionFreeTime += Time.deltaTime;
            if (collisionFreeTime >= CollisionFreeTimeThreshold)
            {
                totalCollisionTime = 0;
            }
        }
        alreadyIncludedCollision = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeManager : MonoBehaviour
{
    public Transform innerCircle;
    public Transform outerCircle;
    public PlayerController player;
    private bool glowingCircle = false;

    public float chargePercentage = 0f;
    public bool doingAction = false;

    private ActionChecker actionChecker = new ActionChecker();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set inner circle.
        chargePercentage = (player.moveSpeed - player.minSpeed) / (player.maxSpeed - player.minSpeed);
        innerCircle.localScale = new Vector3(chargePercentage, chargePercentage);
        if (chargePercentage == 1 && !glowingCircle && player.state == PlayerController.State.Normal)
        {
            innerCircle.GetComponent<Image>().color = new Color(255, 255, 0, 135); // Yellow.
            glowingCircle = true;
        }

        // Check for inputs.
        if (!doingAction && chargePercentage == 1 && player.state == PlayerController.State.Normal && Input.GetMouseButtonDown(0))
        {
            Ray r = player.Camera.ScreenPointToRay(Input.mousePosition);
            player.PositionPlane.Raycast(r, out float distanceToPoint);
            float distanceFromPlayer = (r.GetPoint(distanceToPoint) - transform.position).magnitude;

            if (distanceFromPlayer < outerCircle.GetComponent<RectTransform>().rect.width/2)
            {
                // Player started touch within the outer circle.
                doingAction = true;
                actionChecker.Reset(player);
            }
        }

        if (doingAction)
        {
            doingAction = actionChecker.Update();
        }

    }

    /// <summary>
    /// To be called from the player when spin starts.
    /// </summary>
    public void ActivateSpin()
    {
        innerCircle.GetComponent<Image>().color = new Color(135, 135, 135, 135); // Grey.
        glowingCircle = false;
    }

    private class ActionChecker
    {
        private float timer = 0.0f;
        public float spinWindupTime = 0.2f;
        public float angleChangePerSecondRequirement = 180f;
        Vector3 prevAngle = Vector3.zero;

        private PlayerController player;

        public bool Update()
        {
            if (Input.GetMouseButtonUp(0)) // User let go of touch.
                return false;

            Ray r = player.Camera.ScreenPointToRay(Input.mousePosition);
            player.PositionPlane.Raycast(r, out float distanceToPoint);
            Vector3 angle = r.GetPoint(distanceToPoint) - player.transform.position;

            // Check increase since previous update.
            float angleChange = Vector3.Angle(angle, prevAngle);
            prevAngle = angle;

            if (angleChange < angleChangePerSecondRequirement * Time.deltaTime)
            {
                timer = 0.0f;
            }
            else
            {
                timer += Time.deltaTime;
                if (timer > spinWindupTime)
                {
                    player.SetState(PlayerController.State.Spinning);
                    return false;
                }
            }

            return true;
        }

        public void Reset(PlayerController player)
        {
            this.player = player;
            timer = 0.0f;
        }
    }
}

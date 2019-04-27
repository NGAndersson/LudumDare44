using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeManager : MonoBehaviour
{
    public Transform innerCircle;
    public Transform outerCirlce;
    public PlayerController player;
    private bool glowingCircle = false;

    public float chargePercentage = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chargePercentage = (player.moveSpeed - player.minSpeed) / (player.maxSpeed - player.minSpeed);
        innerCircle.localScale = new Vector3(chargePercentage, chargePercentage);
        if (chargePercentage >= 0.999f && !glowingCircle)
        {
            innerCircle.GetComponent<Image>().color = Color.yellow;
            glowingCircle = true;
        }
    }

    /// <summary>
    /// To be called from the player.
    /// </summary>
    public void ActivateSpin()
    {
        innerCircle.GetComponent<Image>().color = Color.gray;
        glowingCircle = false;
    }
}

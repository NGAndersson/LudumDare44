using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretcher : MonoBehaviour
{
    public Animator animController;

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.position) < 10f)
        {
            animController.SetBool("Stretch", true);
        }
        else
        {
            animController.SetBool("Stretch", false);
        }
    }
}

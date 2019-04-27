using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The gameobject for the player.
    /// </summary>
    public Transform playerTransform;

    /// <summary>
    /// The position offset from the player.
    /// </summary>
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, playerTransform.position + offset, 0.9f);
        this.transform.LookAt(playerTransform);
    }
}

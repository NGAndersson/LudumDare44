using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samboni : MonoBehaviour
{
    public Transform[] checkPoints;

    private int currIndex = 0;

    private float startYPos;

    // Start is called before the first frame update
    void Start()
    {
        startYPos = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = this.transform.forward * 10;
        this.transform.position = new Vector3(this.transform.position.x, startYPos, this.transform.position.z);
        //this.transform.Translate(-this.transform.right * Time.deltaTime * 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Rink")
        {
            //Vector3 dir = checkPoints[currIndex].position;
            this.transform.LookAt(checkPoints[currIndex].position);

            currIndex++;

            if (currIndex >= checkPoints.Length)
            {
                currIndex = 0;
            }
        }
    }
}

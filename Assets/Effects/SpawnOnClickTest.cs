using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClickTest : MonoBehaviour
{
    public SpawnEffect spawnEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = Instantiate(spawnEffect).gameObject;
            go.transform.position = this.transform.position;

            go.GetComponent<SpawnEffect>().Spawn(Vector3.forward, 10);
        }
    }
}

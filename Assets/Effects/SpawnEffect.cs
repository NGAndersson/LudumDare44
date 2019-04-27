using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public Vector2 nrSpawnObjectVariation = new Vector2(2, 4);
    public RigidPart[] spawnObjectPrefabs;


    private List<RigidPart> spawnObjects = new List<RigidPart>();
    private ParticleSystem ps;

    private void Awake()
    {
        int nrToSpawn = Random.Range((int)nrSpawnObjectVariation.x, (int)nrSpawnObjectVariation.y);

        for (int i = 0; i < nrToSpawn; i++)
        {
            RigidPart toSpawn = spawnObjectPrefabs[Random.Range(0, spawnObjectPrefabs.Length - 1)];
            GameObject go = Instantiate(toSpawn.gameObject);

            go.SetActive(false);
            spawnObjects.Add(go.GetComponent<RigidPart>());
        }
    }

    public void Spawn(Vector3 direction, float force)
    {
        for (int i = 0; i < spawnObjects.Count; i++)
        {
            spawnObjects[i].transform.position = this.transform.position;
            spawnObjects[i].Spawn(direction, force);
        }

        // Direct the particles, assumes that the particlesystem is pointing forward.
        this.transform.forward = direction;

        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidPart : MonoBehaviour
{
    public Rigidbody rBody;

    public void Spawn(Vector3 direction, float force)
    {
        this.gameObject.SetActive(true);

        force = force * Random.Range(0.7f, 1.3f);

        float posOffset = Random.Range(-1, 1);
        this.transform.localPosition += new Vector3(posOffset, 0, posOffset);

        Vector3 randomVector = new Vector3(Random.value, Random.value, Random.value).normalized;
        this.transform.localRotation = Quaternion.Euler(randomVector);

        float scaleMult = Random.Range(0.6f, 2f);
        this.transform.localScale = this.transform.localScale * scaleMult;

        this.rBody.AddForce((direction + (randomVector * 0.5f)) * force, ForceMode.Impulse);
        this.rBody.AddTorque(randomVector * force, ForceMode.Impulse);

        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(3);
        rBody.isKinematic = true;
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
    new Rigidbody rigidbody;
    PlayerController targetPlayer;
    Rigidbody targetPlayerRigidbody;
    Transform currentTarget;

    float speed = 2f;

    bool isAiEnabled = false;

    public void Initialize(PlayerController playerController, Rigidbody playerRigidbody)
    {
        targetPlayer = playerController;
        targetPlayerRigidbody = playerRigidbody;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPlayerRigidbody.rotation, 1.0f);
        ToggleCollision(false);
    }

    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        Assert.IsNotNull(rigidbody);
        rigidbody.useGravity = false;
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // TODO just an example
        Vector3 direction = currentTarget.position - transform.position;
        direction = direction.ResetY();
        direction.Normalize();
        transform.SetPositionAndRotation(transform.position + (direction * speed * Time.deltaTime), transform.rotation);
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ReleaseFromSpawner()
    {
        isAiEnabled = true;
        ToggleCollision(true);
        rigidbody.useGravity = true;
        SetTarget(targetPlayerRigidbody.transform);
    }

    void ToggleCollision(bool collionEnabled)
    {
        // TODO prefetch the boxcoll
        gameObject.GetComponent<BoxCollider>().enabled = collionEnabled;
    }
}

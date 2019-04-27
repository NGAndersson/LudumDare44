using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Enemy : MonoBehaviour
{
    new Rigidbody rigidbody;
    PlayerController targetPlayer;
    Rigidbody targetPlayerRigidbody;
    Transform currentTarget;

    const float MaxSpeed = 500f;
    const float MinSpeed = 1000f;
    float speed = MinSpeed;

    public abstract int PointValue { get; }

    public void Initialize(PlayerController playerController, Rigidbody playerRigidbody)
    {
        targetPlayer = playerController;
        targetPlayerRigidbody = playerRigidbody;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetPlayerRigidbody.rotation, 1.0f);
        IgnoreCollisions(true);
    }

    void Start()
    {
        speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        rigidbody = GetComponentInParent<Rigidbody>();
        Assert.IsNotNull(rigidbody);
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = currentTarget.position - transform.position;
        direction = direction.ResetY();
        direction.Normalize();
        rigidbody.AddForce(direction * speed * Time.deltaTime);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity.normalized), Time.deltaTime * 5f);
    }

    public abstract void Die();

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ReleaseFromSpawner()
    {
        IgnoreCollisions(false);
        SetTarget(targetPlayerRigidbody.transform);
    }

    void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Rink"), LayerMask.NameToLayer("Enemy"), ignore);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Rink"), ignore);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), ignore);
    }
}


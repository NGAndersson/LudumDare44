using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Enemy : MonoBehaviour
{
    public SpawnEffect spawnEffect;

    new Rigidbody rigidbody;
    Rigidbody targetPlayerRigidbody;
    Transform currentTarget;
    BoxCollider boxCollider;
    Transform target = null;

    const float InitialSpeed = 5f;
    const float MaxSpeed = 400f;
    const float MinSpeed = 800f;
    float speed = MinSpeed;

    Vector2 maxVelocityRange = new Vector2(2, 18);
    float maxVelocity;
    float increaseVelTimer = 0.0f;

    public abstract int PointValue { get; }

    public void Initialize()
    {
    }

    void Start()
    {
        speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        rigidbody = GetComponentInParent<Rigidbody>();
        boxCollider = GetComponentInParent<BoxCollider>();
        Assert.IsNotNull(rigidbody);
        IgnoreCollisions(true);

        float randomSize = UnityEngine.Random.Range(0.8f, 1.2f);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        maxVelocity = UnityEngine.Random.Range(maxVelocityRange.x, maxVelocityRange.x + 3);
    }

    void Update()
    {
        if (increaseVelTimer < Time.time && maxVelocity < maxVelocityRange.y)
        {
            increaseVelTimer = 6f + Time.time;
            maxVelocity++;
        }

        MoveTowardsTarget();

        if (rigidbody.velocity.magnitude > maxVelocity)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = currentTarget.position - transform.position;
        direction = direction.ResetY();
        direction.Normalize();
        rigidbody.AddForce(direction * speed * Time.deltaTime);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rigidbody.velocity.normalized), Time.deltaTime * 5f);
    }

    public abstract void Die(Vector3 deathVector);

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ReleaseFromSpawner()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerController>().transform;
        }
        SetTarget(target);
        IgnoreCollisions(false);
    }

    void IgnoreCollisions(bool ignore)
    {
        boxCollider.enabled = !ignore;
        rigidbody.useGravity = !ignore;
    }

    public virtual void DeathEffect(Vector3 direction)
    {
        GameObject go = Instantiate(spawnEffect.gameObject);
        go.transform.position = transform.position;
        go.GetComponent<SpawnEffect>().Spawn(direction.normalized, 10);
    }
}


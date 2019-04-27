using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
    new Rigidbody rigidbody;
    PlayerController targetPlayer;
    Rigidbody targetPlayerRigidbody;

    float speed = 2f;

    public void Initialize(PlayerController playerController, Rigidbody playerRigidbody)
    {
        targetPlayer = playerController;
        targetPlayerRigidbody = playerRigidbody;
    }

    private void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        Assert.IsNotNull(rigidbody);
    }

    void Update()
    {
        // TODO just an example
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        // TODO just an example
        Vector3 direction = targetPlayerRigidbody.transform.position - transform.position;
        direction = direction.ResetY();
        direction.Normalize();
        transform.SetPositionAndRotation(transform.position + (direction * speed * Time.deltaTime), transform.rotation);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}

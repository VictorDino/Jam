using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleBounce : MonoBehaviour
{
    Rigidbody2D rb;

    Vector3 lastVelocity;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float speed = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        rb.velocity = direction * Mathf.Max(speed, 0f);
    }
}
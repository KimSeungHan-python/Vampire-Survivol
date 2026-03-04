using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D detector;
    public float pullSpeed;

    //Check if the other game object has the ICollectible interface
    

    void Start()
    {
        // player = FindObjectOfType<PlayerStats>();
    // playerCollider = GetComponent<CircleCollider2D>();
        player = GetComponentInParent<PlayerStats>();
    }

    // void Update()
    // {
    //     playerCollider.radius = player.CurrentMagnet;
    // }

    public void SetRadius(float radius)
    {
        if (!detector)
        {
            detector = GetComponent<CircleCollider2D>();
            detector.radius = radius;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other GameObject is a Pickup.
        if(other.TryGetComponent(out Pickup pickup))
        {
            // Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            // Vector2 forceDirection = (transform.position - other.transform.position).normalized;
            // rb.AddForce(forceDirection * pullSpeed);
            // //if it does, call the Collect method
            pickup.Collect(player, pullSpeed);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollider;
    public float pullSpeed;

    //Check if the other game object has the ICollectible interface
    

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    void update()
    {
        playerCollider.radius = player.currentMagnet;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<ICollectible>(out ICollectible collectible))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);
            //if it does, call the Collect method
            collectible.Collect();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Pickup : MonoBehaviour, ICollectible
{
    protected bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

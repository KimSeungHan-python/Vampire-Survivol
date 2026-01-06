using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Pickup : MonoBehaviour
{
        private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

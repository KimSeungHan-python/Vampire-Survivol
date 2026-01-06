using UnityEngine;
using System.Collections.Generic;
using System.Collections; 
public class BreakableProps : MonoBehaviour
{
    public float health;
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Kill();
        }   
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}

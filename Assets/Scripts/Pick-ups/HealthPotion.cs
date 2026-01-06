using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HealthPotion : Pickup, ICollectible
{
    public float healthRestore;

    public void Collect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.RestoreHealth(healthRestore);
    }
}

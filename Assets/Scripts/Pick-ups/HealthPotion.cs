using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HealthPotion : Pickup
{
    public float healthRestore;

    public override void Collect()
    {
        if(!hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.RestoreHealth(healthRestore);
    }
}

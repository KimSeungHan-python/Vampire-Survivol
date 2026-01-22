using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
public class ExperienceGem : Pickup
{
    
    public int experienceGranted;
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
        playerStats.IncreaseExperience(experienceGranted);

    }

}

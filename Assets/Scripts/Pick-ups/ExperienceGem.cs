using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
public class ExperienceGem : Pickup, ICollectible
{
    
    public int experienceGranted;
    public void Collect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.IncreaseExperience(experienceGranted);

    }

}

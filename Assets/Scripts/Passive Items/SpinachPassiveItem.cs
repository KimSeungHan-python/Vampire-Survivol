using UnityEngine;
using System.Collections.Generic;
using System.Collections;  
public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMight *= 1 + passiveItemData.Multipler / 100f;
    }
}

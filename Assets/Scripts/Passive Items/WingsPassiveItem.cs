using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
public class WingsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMoveSpeed *= 1 + passiveItemData.Multipler / 100f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multiplier / 100f;   //Increases specifically the might, for the powder item
    }
}

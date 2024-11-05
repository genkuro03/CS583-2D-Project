using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;   //Increasees the moveSpeed my item multipier
    }

}

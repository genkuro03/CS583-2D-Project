using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;   //Grabs player data
    public PassiveItemScriptableObject passiveItemData; //Grabs item data

    protected virtual void ApplyModifier()  //Skeleton of applyModifier (this is base class all children will overwrite)
    {

    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();   //Grabs player stats
        ApplyModifier();    //Applys the modifier, depending what item / script it is
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName ="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject //Crreated scriptable objects to allow verticality of new items
{

    //Lamda functions to shorthand getters/setters 
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int level;

    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;

    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}

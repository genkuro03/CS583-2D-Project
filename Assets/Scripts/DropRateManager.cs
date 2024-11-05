using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour    //Responsible for managing drop rate on enemies
{
    [System.Serializable]
    public class Drops  //Items for inspector to add mutliple types of drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;   //List of possible drops

    void OnDestroy()        
    {
        if (!gameObject.scene.isLoaded) //Checks if game object is still in scene
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0f, 100f);    //Determines which item should drop
        List<Drops> possibleDrops = new List<Drops>();  //Creates list of every drop in this cycle (prevents multiple drops)

        foreach (Drops rate in drops)   //iterates throguh drop
        {
            if(randomNumber <= rate.dropRate)   //if # is <= drop rate add to list
            {
                possibleDrops.Add(rate);    
            }
        }
        if(possibleDrops.Count > 0) //if at least one drop, choose one at random
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity); //Spawn drop
        }

    }
}

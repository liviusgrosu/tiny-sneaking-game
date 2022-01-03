using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    public LootManager LootManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LootManager.GetRating() != 0)
            {
                Debug.Log("Level passed");
            }
            else
            {
                Debug.Log("Not enough loot");
            }
        }
    }
}

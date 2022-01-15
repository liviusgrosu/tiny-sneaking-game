using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    public LootManager LootManager;
    public GameManager GameManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LootManager.GetRating() != 0)
            {
                GameManager.WinGame();
            }
        }
    }
}

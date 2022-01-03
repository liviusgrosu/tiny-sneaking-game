using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    public LootManager LootManager;
    public GameWinScreen GameWinScreen;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (LootManager.GetRating() != 0)
            {
                GameWinScreen.ToggleUI();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [SerializeField] private LootManager _lootManager;
    [SerializeField] private GameManager _gameManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // If no rating can be given then the player hasn't completed enough loot
            if (_lootManager.GetRating() != 0)
            {
                _gameManager.WinGame();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PickUpLoot : MonoBehaviour
{
    private LootManager _lootManager;
    private List<Transform> _availableLoot = new List<Transform>();
    void Start()
    {
        _lootManager = GameObject.Find("Game Manager").GetComponent<LootManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        // When its close, add the loot to the list of available loot to pickup
        if (col.tag == "Loot" && !_availableLoot.Contains(col.transform))
        {
            _availableLoot.Add(col.transform);
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        // When its far, remove the loot to the list of available loot to pickup
        if (col.tag == "Loot" && _availableLoot.Contains(col.transform))
        {
            _availableLoot.Remove(col.transform);
        }
    }

    void Update()
    {
        // Check to avoid picking up multiple loot drops
        if (Input.GetKeyDown(KeyCode.Space) && _availableLoot.Count != 0)
        {
            Transform closestLoot = _availableLoot[0];
            // Find the closest loot   
            foreach(Transform loot in _availableLoot.Skip(1))
            {
                if (Vector3.Distance(transform.position, loot.position) < Vector3.Distance(transform.position, closestLoot.position))
                {
                    closestLoot = loot;
                }
            }
            // Pick up the closest loot
            _lootManager.AddLoot(closestLoot.GetComponent<LootObject>().Score);
            _availableLoot.Remove(closestLoot);
            Destroy(closestLoot.gameObject);
        }
    }
}

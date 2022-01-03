using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LootManager : MonoBehaviour
{
    public List<Transform> AvailableLoot = new List<Transform>();
    private int _score;
    public Text ScoreText;

    void OnTriggerEnter(Collider col)
    {
        // When its close, add the loot to the list of available loot to pickup
        if (col.tag == "Loot" && !AvailableLoot.Contains(col.transform))
        {
            AvailableLoot.Add(col.transform);
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        // When its far, remove the loot to the list of available loot to pickup
        if (col.tag == "Loot" && AvailableLoot.Contains(col.transform))
        {
            AvailableLoot.Remove(col.transform);
        }
    }

    void Update()
    {
        // Check to avoid picking up multiple loot drops
        if (Input.GetKeyDown(KeyCode.Space) && AvailableLoot.Count != 0)
        {
            Transform closestLoot = AvailableLoot[0];
            // Find the closest loot   
            foreach(Transform loot in AvailableLoot.Skip(1))
            {
                if (Vector3.Distance(transform.position, loot.position) < Vector3.Distance(transform.position, closestLoot.position))
                {
                    closestLoot = loot;
                }
            }
            // Pick up the closest loot
            _score += closestLoot.GetComponent<LootObject>().Score;
            ScoreText.text = _score.ToString();
            AvailableLoot.Remove(closestLoot);
            Destroy(closestLoot.gameObject);
        }
    }
}

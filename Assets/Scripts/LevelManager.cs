using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    // In percentage
    [Tooltip("Minimum amount of loot required to finish the level")]
    [Range(0.0f, 100.0f)]
    public float LootMinPercent;
    private int _totalLoot;
    private int _minLootScore;

    void Start()
    {
        // Get the maps total score
        GameObject[] lootObj = GameObject.FindGameObjectsWithTag("Loot");
        lootObj.ToList().ForEach(x => _totalLoot += x.GetComponent<LootObject>().Score);

        // Get the min. loot required to beat the level
        _minLootScore = (int)(_totalLoot * (LootMinPercent / 100.0f));
    }
}

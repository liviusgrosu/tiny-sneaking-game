using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LootManager : MonoBehaviour
{
    [Tooltip("Minimum amount of loot required to finish the level")]
    [Range(0.0f, 100.0f)]
    [SerializeField] private float _lootMinPercent;
    private int _minLootScore, _midLootScore, _maxLootScore;
    private int _currentScore;
    [SerializeField] private Text _scoreText;

    void Start()
    {
        // Get the total amount of loot in the level   
        GameObject[] lootObj = GameObject.FindGameObjectsWithTag("Loot");
        lootObj.ToList().ForEach(x => _maxLootScore += x.GetComponent<LootObject>().Score);

        // Get the min and mid loot required to beat the level
        _minLootScore = (int)(_maxLootScore * (_lootMinPercent / 100.0f));
        _midLootScore = (int)((_maxLootScore - _minLootScore) / 2.0f);

        // Update the score text
        _scoreText.text = $"Loot: 0/{_minLootScore}";
    }


    public void AddLoot(int amount)
    {
        // Add the loot to the score and update the text
        _currentScore += amount;
        _scoreText.text = $"Loot: {_currentScore}/{_minLootScore}";
    }

    public int GetRating()
    {
        // Not enough loot
        if (_currentScore < _minLootScore)
        {
            return 0;
        }

        // 1 star rating
        if (_currentScore >= _minLootScore && _currentScore < _midLootScore)
        {
            return 1;
        }

        // 2 star rating
        if (_currentScore >= _midLootScore && _currentScore < _maxLootScore)
        {
            return 2;
        }

        // 3 star rating
        return 3;
    }

    public void ToggleUI(bool state)
    {
        // Toggle the UI visibility
        _scoreText.gameObject.SetActive(state);
    }
}

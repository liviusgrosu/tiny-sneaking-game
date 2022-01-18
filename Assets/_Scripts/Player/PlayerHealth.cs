using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    [HideInInspector] public int CurrentHealth;
    private GameManager _gameManager;

    public void Start()
    {
        CurrentHealth = StartingHealth;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void ReduceHealth(int amount)
    {
        // Lose the game when health goes to 0 or below
        if (CurrentHealth <= amount)
        {
            // Don't go past 0
            CurrentHealth = 0;
            _gameManager.LoseGame();
            return;
        }
        // Lose health
        CurrentHealth -= amount;
    }
}

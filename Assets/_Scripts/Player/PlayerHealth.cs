using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    [HideInInspector]
    public int CurrentHealth;
    private GameManager GameManager;

    public void Initialize(GameManager gameManager)
    {
        CurrentHealth = StartingHealth;
        GameManager = gameManager;
    }

    public void ReduceHealth(int amount)
    {
        if (CurrentHealth <= amount)
        {
            // Don't go past 0
            CurrentHealth = 0;

            GameManager.LoseGame();

            return;
        }
        CurrentHealth -= amount;
    }
}

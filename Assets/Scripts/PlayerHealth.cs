using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    [HideInInspector]
    public int CurrentHealth;
    private GameOverScreen GameOverUI;

    public void Initialize(GameOverScreen gameOverUI)
    {
        CurrentHealth = StartingHealth;
        GameOverUI = gameOverUI;
    }

    public void ReduceHealth(int amount)
    {
        if (CurrentHealth <= amount)
        {
            // Don't go past 0
            CurrentHealth = 0;

            GameOverUI.ToggleUI();

            return;
        }
        CurrentHealth -= amount;
    }
}

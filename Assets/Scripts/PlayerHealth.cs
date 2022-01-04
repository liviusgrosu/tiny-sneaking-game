using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    [HideInInspector]
    public int CurrentHealth;

    void Awake()
    {
        CurrentHealth = StartingHealth;
    }

    public void ReduceHealth(int amount)
    {
        if (CurrentHealth < amount)
        {
            // Don't go past 0
            CurrentHealth = 0;
            return;
        }
        CurrentHealth -= amount;
    }

    private void Update()
    {
        
    }
}

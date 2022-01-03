using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    private int _currentHealth;

    void Awake()
    {
        _currentHealth = StartingHealth;
    }

    public void ReduceHealth(int amount)
    {
        
    }
}

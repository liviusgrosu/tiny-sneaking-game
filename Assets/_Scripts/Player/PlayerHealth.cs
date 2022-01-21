using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int StartingHealth;
    [HideInInspector] public int CurrentHealth;
    private GameManager _gameManager;
    private Animator _animator;

    private void Start()
    {
        CurrentHealth = StartingHealth;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ReduceHealth(StartingHealth);
        }
    }

    public void ReduceHealth(int amount)
    {
        // Lose the game when health goes to 0 or below
        if (!IsDead() && CurrentHealth <= amount)
        {
            // Don't go past 0
            CurrentHealth = 0;
            _animator.SetTrigger("Die");
            _gameManager.LoseGame();
            return;
        }
        // Lose health
        CurrentHealth -= amount;
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }
}

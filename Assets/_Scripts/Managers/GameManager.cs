using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [HideInInspector] public Transform PlayerInstance;
    [SerializeField] private Transform _spawn;
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    [SerializeField] private LootManager _lootManager;
    [SerializeField] private GameOverScreen _gameOverUI;
    [SerializeField] private GameWinScreen _gameWinScreen;
    [SerializeField] private PauseScreen _pauseScreen;
    private string _currentSceneName;

    // TODO: add player health here so that difficulty can affect that

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(_playerPrefab, _spawn.position, _playerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
        _playerHealthUI.InitilizeUI(PlayerInstance.GetComponent<PlayerHealth>());
        _currentSceneName = SceneManager.GetActiveScene().name;

        //HideAllHideables();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Pause the game
            PauseGame();
        }
    }

    public void RestartLevel()
    {
        // Reload the scene
        Time.timeScale = 1;
        SceneManager.LoadScene(_currentSceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        // Hide the overlay
        ToggleOverlay(false);
        // Show the pause screen
        _pauseScreen.ToggleUI(true);
    }

    public void ResumeGame()
    {
        // Continue the scene
        Time.timeScale = 1;
        _pauseScreen.ToggleUI(false);
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        // Hide the overlay
        ToggleOverlay(false);
        // Show the game win screen
        _gameWinScreen.ToggleUI();
    }

    public void LoseGame()
    {
        Time.timeScale = 1;
        // Hide the overlay
        ToggleOverlay(false);
        // Show the game lose screen
        _gameOverUI.ToggleUI(true);
    }

    public void AdvanceLevel()
    {
        Time.timeScale = 1;
        int currentLevel = Int32.Parse(SceneManager.GetActiveScene().name.Split().Last());
        SceneManager.LoadScene($"Test Scene {currentLevel + 1}");
    }

    public void ChangeToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void ToggleOverlay(bool state)
    {
        // Hide the UI overlay
        _playerHealthUI.ToggleUI(state);
        _lootManager.ToggleUI(state);
    }

    private void HideAllHideables()
    {
        // Hide all things that need to be hid from the player
        Hideable[] hideables = FindObjectsOfType(typeof(Hideable)) as Hideable[];

        foreach(Hideable obj in hideables)
        {
            obj.ToggleState(false);
        }
    }

    public bool IsPlayerDead()
    {
        return PlayerInstance.GetComponent<PlayerHealth>().IsDead();
    }
}

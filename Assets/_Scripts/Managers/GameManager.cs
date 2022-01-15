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
    public LootManager LootManager;
    public GameOverScreen GameOverUI;
    public GameWinScreen GameWinScreen;
    public PauseScreen PauseScreen;
    private string _currentSceneName;

    //TODO add player health here so that difficulty can affect that

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(_playerPrefab, _spawn.position, _playerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
        PlayerInstance.GetComponent<PlayerHealth>().Initialize(this);
        _playerHealthUI.InitilizeUI(PlayerInstance.GetComponent<PlayerHealth>());

        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        PauseScreen.ToggleUI(true);
    }

    public void ResumeGame()
    {
        // Continue the scene
        Time.timeScale = 1;
        PauseScreen.ToggleUI(false);
    }

    public void WinGame()
    {
        // Hide the overlay
        ToggleOverlay(false);
        // Show the game win screen
        GameWinScreen.ToggleUI();
    }

    public void LoseGame()
    {
        // Hide the overlay
        ToggleOverlay(false);
        // Show the game lose screen
        GameOverUI.ToggleUI(true);
    }

    public void AdvanceLevel()
    {
        
    }

    public void ChangeToMainMenu()
    {

    }

    public void ToggleOverlay(bool state)
    {
        _playerHealthUI.ToggleUI(state);
        LootManager.ToggleUI(state);
    }

    private void ToggleUIElement(Transform element)
    {

    }
}

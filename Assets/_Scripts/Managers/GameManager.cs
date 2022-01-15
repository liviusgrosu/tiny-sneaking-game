using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    [HideInInspector]
    public Transform PlayerInstance;
    public Transform Spawn;
    public PlayerHealthUI PlayerHealthUI;
    public LootManager LootManager;
    public GameOverScreen GameOverUI;
    public GameWinScreen GameWinScreen;
    public PauseScreen PauseScreen;
    private string _currentSceneName;

    //TODO add player health here so that difficulty can affect that

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(PlayerPrefab, Spawn.position, PlayerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
        PlayerInstance.GetComponent<PlayerHealth>().Initialize(this);
        PlayerHealthUI.InitilizeUI(PlayerInstance.GetComponent<PlayerHealth>());

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
        Time.timeScale = 1;
        SceneManager.LoadScene(_currentSceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseScreen.ToggleUI(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseScreen.ToggleUI(false);
    }

    public void WinGame()
    {
        GameWinScreen.ToggleUI();
    }

    public void LoseGame()
    {
        GameOverUI.ToggleUI();
    }

    public void AdvanceLevel()
    {
        
    }

    public void ChangeToMainMenu()
    {

    }

    public void ToggleOverlay(bool state)
    {
        PlayerHealthUI.ToggleUI(state);
        LootManager.ToggleUI(state);
    }
}

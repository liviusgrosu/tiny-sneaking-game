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
    private string _currentSceneName;

    //TODO add player health here so that difficulty can affect that

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(PlayerPrefab, Spawn.position, PlayerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
        PlayerInstance.GetComponent<PlayerHealth>().Initialize(GameOverUI);
        PlayerHealthUI.InitilizeUI(PlayerInstance.GetComponent<PlayerHealth>());

        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_currentSceneName);
    }

    public void AdvanceLevel()
    {

    }

    public void ChangeToMainMenu()
    {

    }

    public void ToggleOverlayOff()
    {
        PlayerHealthUI.ToggleUI(false);
        LootManager.ToggleUI(false);
    }
}

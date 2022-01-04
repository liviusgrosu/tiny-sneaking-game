using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    [HideInInspector]
    public Transform PlayerInstance;
    public Transform Spawn;
    public PlayerHealthUI PlayerHealthUI;
    public GameOverScreen GameOverUI;

    //TODO add player health here so that difficulty can affect that

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(PlayerPrefab, Spawn.position, PlayerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
        PlayerInstance.GetComponent<PlayerHealth>().Initialize(GameOverUI);
        PlayerHealthUI.InitilizeUI(PlayerInstance.GetComponent<PlayerHealth>());
    }

    public void RestartLevel()
    {
        
    }

    public void AdvanceLevel()
    {

    }

    public void ChangeToMainMenu()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerInstance;
    public Transform Spawn;

    void Start()
    {
        // Spawn the player in
        GameObject playerParent = Instantiate(PlayerPrefab, Spawn.position, PlayerPrefab.transform.rotation);
        PlayerInstance = playerParent.transform.Find("Player");
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

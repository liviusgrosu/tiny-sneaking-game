using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform Spawn;

    void Start()
    {
        // Spawn the player in
        Instantiate(PlayerPrefab, Spawn.position, PlayerPrefab.transform.rotation);
    }
}

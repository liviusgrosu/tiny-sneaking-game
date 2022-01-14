using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public GameManager GameManager;

    public void ReplayButtonPress()
    {
        GameManager.RestartLevel();
    }

    public void MenuButtonPress()
    {
        GameManager.ChangeToMainMenu();
    }

    public void ToggleUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        // Toggle off the other UI
        GameManager.ToggleOverlay(false);
    }
}

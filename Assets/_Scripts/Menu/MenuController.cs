using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu, _levelMenu;

    public void PressLevelsButton()
    {
        _mainMenu.SetActive(false);
        _levelMenu.SetActive(true);
    }

    public void PressLevelButton(int levelId)
    {
        // Change scene name when selecting a level
        SceneManager.LoadScene($"Test Level {levelId}");
    }

    public void PressBackButton()
    {
        _mainMenu.SetActive(true);
        _levelMenu.SetActive(false);
    }

    public void PressExitButton()
    {
        // Quit the game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

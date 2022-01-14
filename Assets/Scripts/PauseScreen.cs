using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GameManager GameManager;

    public void ToggleUI(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }

        // Toggle off the other UI
        GameManager.ToggleOverlay(!state);
    }
}

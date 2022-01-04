using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject Heart;
    public RectTransform HeartsParent;
    private List<GameObject> _hearts;
    private PlayerHealth _playerHealth;

    public void InitilizeUI(PlayerHealth playerHealth)
    {
        // The spacing between each heart
        _playerHealth = playerHealth;
        float incrementX = HeartsParent.rect.width / _playerHealth.StartingHealth;

        _hearts = new List<GameObject>();

        // Place the hearts at the beginning of the container parent
        float startingX = HeartsParent.rect.width / 2.0f;
        Vector3 startingPos = HeartsParent.position - new Vector3(startingX, 0, 0);

        for (int i = 0; i < _playerHealth.StartingHealth; i++)
        {
            // Instantiate the hearts
            GameObject heart = Instantiate(Heart, startingPos + new Vector3(incrementX * i, 0, 0), Heart.transform.rotation);
            heart.transform.SetParent(HeartsParent);
            _hearts.Add(heart);
        }
    }

    private void Update()
    {
        if (_hearts.Count != _playerHealth.CurrentHealth)
        {
            int difference = _hearts.Count - _playerHealth.CurrentHealth;
            // For now only deal with loosing health
            for (int i = _hearts.Count - 1; i >= _playerHealth.CurrentHealth; i--)
            {
                GameObject heart = _hearts[i];
                _hearts.RemoveAt(i);
                Destroy(heart);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject _heart;
    [SerializeField] private GameObject _label;
    [SerializeField] private RectTransform _heartsParent;
    private List<GameObject> _hearts;
    private PlayerHealth _playerHealth;

    public void InitilizeUI(PlayerHealth playerHealth)
    {
        // The spacing between each heart
        _playerHealth = playerHealth;
        float incrementX = _heartsParent.rect.width / _playerHealth.StartingHealth;

        _hearts = new List<GameObject>();

        // Place the hearts at the beginning of the container parent
        float startingX = _heartsParent.rect.width / 2.0f;
        Vector3 startingPos = _heartsParent.position - new Vector3(startingX, 0, 0);

        for (int i = 0; i < _playerHealth.StartingHealth; i++)
        {
            // Instantiate the hearts
            GameObject heart = Instantiate(_heart, startingPos + new Vector3(incrementX * i, 0, 0), _heart.transform.rotation);
            heart.transform.SetParent(_heartsParent);
            _hearts.Add(heart);
        }
    }

    private void LateUpdate()
    {
        if (_hearts.Count > 0 && _hearts.Count != _playerHealth.CurrentHealth)
        {
            int difference = _hearts.Count - _playerHealth.CurrentHealth;
            // Remove hearts if there is a change between this scripts health and the actual players health
            for (int i = _hearts.Count - 1; i >= _playerHealth.CurrentHealth; i--)
            {
                GameObject heart = _hearts[i];
                _hearts.RemoveAt(i);
                Destroy(heart);
            }
        }
    }

    public void ToggleUI(bool state)
    {
        _label.SetActive(state);
        _heartsParent.gameObject.SetActive(state);
    }
}

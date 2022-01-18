using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameWinScreen : MonoBehaviour
{
    [SerializeField] private LootManager _lootManager;
    [SerializeField] private List<RectTransform> _stars;
    [SerializeField] private Sprite _fullStar;

    public void ToggleUI()
    {
        // Start showing the scoring stars
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(UnlockStarScore());
    }

    IEnumerator UnlockStarScore()
    {
        // For each star, slowly reveal them
        for (int starIdx = 0; starIdx < _lootManager.GetRating(); starIdx++)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 starPos = _stars.ElementAt(starIdx).transform.position;
            _stars.ElementAt(starIdx).GetComponent<Image>().sprite = _fullStar;
        }
    }
}

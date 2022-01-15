using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameWinScreen : MonoBehaviour
{
    public LootManager LootManager;
    public GameManager GameManager;

    public List<RectTransform> Stars;
    public Sprite FullStar;

    public void ToggleUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        StartCoroutine(UnlockStarScore());
    }

    IEnumerator UnlockStarScore()
    {
        for (int starIdx = 0; starIdx < LootManager.GetRating(); starIdx++)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 starPos = Stars.ElementAt(starIdx).transform.position;
            Stars.ElementAt(starIdx).GetComponent<Image>().sprite = FullStar;
        }
    }
}

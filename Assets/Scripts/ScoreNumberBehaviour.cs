/*
* Author: Lim En Xu Jayson
* Date: 10 December 2025
* Description: Animates transient score text and cleans it up.
*/
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreNumberBehaviour : MonoBehaviour
{
    float timeout=1f;
    /// <summary>
    /// Scales up and fades out the score text, then destroys it.
    /// </summary>
    IEnumerator fadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;
        Vector3 targetScale = startingScale * 1.5f;
        CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
        while (elapsedTime < timeout)
        {
            float t = elapsedTime / timeout;
            transform.localScale = Vector3.Lerp(startingScale, targetScale, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    /// <summary>
    /// Initializes text with current score and starts fade-out.
    /// </summary>
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = GameObject.FindWithTag("ActivePet").GetComponent<Play>().score.ToString();
        StartCoroutine(fadeOutAndDestroy());
        Debug.Log("Score Number Created with score: " + GameObject.FindWithTag("ActivePet").GetComponent<Play>().score.ToString());
    }
}

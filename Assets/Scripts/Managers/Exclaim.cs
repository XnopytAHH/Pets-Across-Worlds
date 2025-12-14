/*
* Author: Lim En Xu Jayson
* Date: 13 December 2025
* Description: Handles exclamation mark effects above pets' heads.
*/
using System.Collections;
using UnityEngine;

public class Exclaim : MonoBehaviour
{
    /// <summary>
    /// Duration before the exclamation mark fades out and is destroyed.
    /// </summary>
    float timeout=1f;

    /// <summary>
    /// Handles the fade-out effect and destroys the exclamation mark object.
    /// </summary>
    IEnumerator fadeOutAndDestroy()
    {
        transform.LookAt(Camera.main.transform);
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
    /// Starts the fade-out coroutine when the exclamation mark is created.
    /// </summary>
    void Start()
    {
        StartCoroutine(fadeOutAndDestroy());
        
    }
}

using System.Collections;
using UnityEngine;

public class Exclaim : MonoBehaviour
{
    float timeout=1f;
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
    void Start()
    {
        StartCoroutine(fadeOutAndDestroy());
        
    }
}

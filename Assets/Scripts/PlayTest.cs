using UnityEngine;
using System.Collections;

public class PlayTest : MonoBehaviour
{
    public float randomScreenX;
    public float randomScreenY;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Canvas canvas;
    public bool tapped = false;
    void Start()
    {
        StartCoroutine(GameLoop());
    }
    IEnumerator GameLoop()
    {
        GameObject tapPosition = null;
        while (true)
        {
            yield return new WaitForSeconds(2);
            randomScreenX = Random.Range(0, Screen.width);
            randomScreenY = Random.Range(0, Screen.height);
            if (tapPosition != null)
            {
                Destroy(tapPosition);
            }
            tapPosition = Instantiate(buttonPrefab, new Vector3(randomScreenX, randomScreenY, 0), Quaternion.identity, canvas.transform);
            yield return StartCoroutine(WaitForTap());
        }
    }
    IEnumerator WaitForTap()
    {
        float tapTimeCounter = 0f;
        tapped = false;
        while (!tapped)
        {
            if (tapTimeCounter >= 2f)
            {
                Debug.Log("Time Out");
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
            tapTimeCounter += 0.1f;
        }
    }
    
}

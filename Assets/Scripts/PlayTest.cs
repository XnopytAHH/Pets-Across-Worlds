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
   void Start()
   {
       StartCoroutine(GameLoop());
   }
    IEnumerator GameLoop()
    {
        while (true)
        {
         Debug.Log("Game Started");
        yield return new WaitForSeconds(2);
        randomScreenX = Random.Range(0,Screen.width);
        randomScreenY = Random.Range(0,Screen.height);
        Instantiate(buttonPrefab, new Vector3(randomScreenX, randomScreenY, 0), Quaternion.identity, canvas.transform);
        }
    }

}

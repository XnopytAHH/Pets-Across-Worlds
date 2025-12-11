using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Play : MonoBehaviour
{
    public float randomScreenX;
    public float randomScreenY;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject BallCatchArea;
    public bool tapped = false;
    [SerializeField]
    GameObject ball;
    public int score = 0;
    float duration = 2.0f;
    void Start()
    {
        canvas = GameObject.Find("GameUI").GetComponent<Canvas>();
    }
    public void StartGame()
    {
        StartCoroutine(GameLoop());
        ball.transform.position = BallCatchArea.transform.position;
        
        canvas.enabled = true;
        ball.GetComponent<MeshRenderer>().enabled = true;
        score = 0;
        duration = 2.0f;

    }
    
    IEnumerator GameLoop()
    {
        GameObject tapPosition = null;
        while (true)
        {
            randomScreenX = Random.Range(0, Screen.width);
            randomScreenY = Random.Range(0, Screen.height);
            ball.transform.position = BallCatchArea.transform.position;
            tapPosition = Instantiate(buttonPrefab, new Vector3(randomScreenX, randomScreenY, 0), Quaternion.identity, canvas.transform);
            StartCoroutine(StartBallTravel(Camera.main.ScreenToWorldPoint(new Vector3(randomScreenX, randomScreenY, 0.5f)), tapPosition, duration));
            yield return WaitForTap();
            if (tapped)
            {
                Debug.Log("Ball Returning");
                StartCoroutine(StartBallReturn(ball.transform.position, duration));
                Destroy(tapPosition);

                yield return new WaitForSeconds(duration);
                if  (score % 5 == 0 && score != 0)
                {
                    duration -= 0.1f;
                    if (duration < 0.5f)
                    {
                        duration = 0.5f;
                    }
                }
                
            }
            else
            {
                
                Destroy(tapPosition);
                ball.GetComponent<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(1);
                GameManager.instance.ShowGameOverUI(score);
                yield break;

            }
            
            
        }
    }
    IEnumerator StartBallTravel(Vector3 targetPosition, GameObject tapPosition, float duration)
    {
        tapped = false;
        tapPosition.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
        Vector3 start = ball.transform.position;
        Vector3 end = targetPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Smooth movement
            ball.transform.position = Vector3.Lerp(start, end, t);
            if (tapped)
            {
                yield break;
            }
            if (elapsed+1f >= duration)
            {
                tapPosition.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, elapsed -1f);
                tapPosition.transform.position = new Vector3(Camera.main.WorldToScreenPoint(ball.transform.position).x,
                    Camera.main.WorldToScreenPoint(ball.transform.position).y, 0);   
            }
            yield return null;
        }

        ball.transform.position = end; // snap to final
        
        

    }
    IEnumerator StartBallReturn(Vector3 targetPosition, float duration)
    {
        
        Vector3 start = targetPosition;
        Vector3 end = BallCatchArea.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Smooth movement
            ball.transform.position = Vector3.Lerp(start, end, t);

            yield return null;
        }

        ball.transform.position = end; // snap to final
        
    }
    IEnumerator WaitForTap()
    {
        float tapTimeCounter = 0f;
        tapped = false;
        while (!tapped)
        {
            if (tapTimeCounter >= duration)
            {
                Debug.Log("Time Out");
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
            tapTimeCounter += 0.1f;
        }
    }

}

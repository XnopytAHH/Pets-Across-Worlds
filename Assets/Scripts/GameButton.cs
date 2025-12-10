using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField]
    GameObject scorePrefab;
    Canvas canvas;
    void Start()
    {
         canvas = GameObject.Find("GameUI").GetComponent<Canvas>();
    }
    public void OnButtonTap()
    {
        GameObject gameController = GameObject.FindWithTag("ActivePet");
        if (gameController != null)
        {
            gameController.GetComponent<Play>().tapped = true;
        }
        GameObject.FindGameObjectWithTag("ActivePet").GetComponent<Play>().score += 1;
        Instantiate(scorePrefab, transform.position, Quaternion.identity, canvas.transform);
        Destroy(gameObject);
    }
}

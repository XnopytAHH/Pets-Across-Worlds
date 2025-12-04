using UnityEngine;

public class GameButton : MonoBehaviour
{
    public void OnButtonTap()
    {
        GameObject gameController = GameObject.FindWithTag("GameplayManager");
        if (gameController != null)
        {
            gameController.GetComponent<PlayTest>().tapped = true;
        }
        Destroy(gameObject);
    }
}

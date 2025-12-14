/*
* Author: Lim En Xu Jayson
* Date: 4 December 2025
* Description: Handles tap input, scoring increment, and ball bounce sound.
*/
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField]
    GameObject scorePrefab;
    Canvas canvas;
    /// <summary>
    /// Caches reference to the game UI canvas.
    /// </summary>
    void Start()
    {
         canvas = GameObject.Find("GameUI").GetComponent<Canvas>();
    }
    /// <summary>
    /// Marks tap, increments score, plays sound, and cleans up.
    /// </summary>
    public void OnButtonTap()
    {
        GameObject gameController = GameObject.FindWithTag("ActivePet");
        if (gameController != null)
        {
            gameController.GetComponent<Play>().tapped = true;
        }
        GameObject.FindGameObjectWithTag("ActivePet").GetComponent<Play>().score += 1;
        Instantiate(scorePrefab, transform.position, Quaternion.identity, canvas.transform);
        
        SoundManager.instance.ballBounce();
        Destroy(gameObject);
    }
}

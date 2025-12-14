/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Rotates world-space UI elements to face the camera horizontally.
*/
using UnityEngine;

public class UIRotationManager : MonoBehaviour
{
  [SerializeField] private RectTransform uiElement;

    // Update is called once per frame
    /// <summary>
    /// Aligns UI rotation toward the player camera on the horizontal plane.
    /// </summary>
    void Update()
    {
        Vector3 angleToPlayer = Camera.main.transform.position - uiElement.position;
        angleToPlayer.y = 0; // Keep only the horizontal angle
        uiElement.rotation = Quaternion.LookRotation(-angleToPlayer);
        
    }
}

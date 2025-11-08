
using UnityEngine;

public class UIRotationManager : MonoBehaviour
{
  [SerializeField] private RectTransform uiElement;

    // Update is called once per frame
    void Update()
    {
        Vector3 angleToPlayer = Camera.main.transform.position - uiElement.position;
        angleToPlayer.y = 0; // Keep only the horizontal angle
        uiElement.rotation = Quaternion.LookRotation(-angleToPlayer);
        
    }
}

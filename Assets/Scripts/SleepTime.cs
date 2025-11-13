using UnityEngine;

public class SleepTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Your pet will wake up at - " + GameManager.instance.sleepTimeTemp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class SleepTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.forcedSleep)
        {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Your pet was too tired and fell asleep! They will wake up in - " + (System.DateTime.Parse(GameManager.instance.currentPlayerPets[GameManager.instance.activePet].fullRestedTime) - System.DateTime.Now).ToString().Substring(0,8);;
        }
        else
        {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Your pet will wake up in - " + (System.DateTime.Parse(GameManager.instance.currentPlayerPets[GameManager.instance.activePet].fullRestedTime) - System.DateTime.Now).ToString().Substring(0,8);;
        }
    }
    public void backToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScreen");
    }
}

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
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Your pet will wake up in - " + (System.DateTime.Parse(GameManager.instance.currentPlayerPets[GameManager.instance.activePet].fullRestedTime) - System.DateTime.Now).ToString().Substring(0,8);;
    }
    public void backToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScreen");
    }
}

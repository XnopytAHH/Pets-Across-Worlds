/*
* Author: Lim En Xu Jayson
* Date: 14 November 2025
* Description: Updates sleep timer UI and handles back navigation.
*/
using UnityEngine;

public class SleepTime : MonoBehaviour
{
    

    
    /// <summary>
    /// Updates the on-screen text with time until pet wakes.
    /// </summary>
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
    /// <summary>
    /// Returns to Login screen and clears active pet.
    /// </summary>
    public void backToMainMenu()
    {
        SoundManager.instance.buttonClick();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScreen");
        GameManager.instance.activePet = null;
    }
}

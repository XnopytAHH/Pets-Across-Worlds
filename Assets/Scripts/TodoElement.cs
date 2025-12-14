/*
* Author: Lim En Xu Jayson
* Date: 12 December 2025
* Description: Controls individual to-do UI items based on pet stats thresholds.
*/
using TMPro;
using UnityEngine;

public class TodoElement : MonoBehaviour
{
    [SerializeField]
    private string StatChecked;
    [SerializeField]
    private string Threshold;
    public bool IsCompleted;

    
   
    /// <summary>
    /// Updates strike-through and color depending on stat thresholds.
    /// </summary>
    void Update()
    {
        if (GameManager.instance.activePet == null) return;
        else if (StatChecked == "Food" && GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel >= float.Parse(Threshold))
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.gray;
            IsCompleted = true;
        }
        else if (StatChecked == "Mood" && GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel >= float.Parse(Threshold))
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.gray;
            IsCompleted = true;
        }
        else if (StatChecked == "Energy" && GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel >= float.Parse(Threshold))
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.gray;
            IsCompleted = true;
        }
        else 
        {
            gameObject.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
            IsCompleted = false;
        }
    }
}

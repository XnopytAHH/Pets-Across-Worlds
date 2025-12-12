using TMPro;
using UnityEngine;

public class TodoElement : MonoBehaviour
{
    [SerializeField]
    private string StatChecked;
    [SerializeField]
    private string Threshold;
    public bool IsCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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

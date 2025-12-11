using System.Collections;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PetStatManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI petName;
    [SerializeField]
    private Slider foodSlider;
    [SerializeField]
    private Slider moodSlider;
    [SerializeField]
    private Slider energySlider;
    [SerializeField]
    private GameObject foodPrefab;
    public bool isPaused = false;
    void Update()
    {
        if (GameManager.instance.activePet == null) return;
        foodSlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel;
        moodSlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel;
        energySlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel;
    }
    void Start()
    {
        StartCoroutine(DecreaseStatsOverTime());
    }
        
    private IEnumerator DecreaseStatsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (isPaused) continue;
            GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel -= 0.1f;
            Debug.Log ("Energy Level Decreased to: " + GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel);
        }
    }
    public void UpdatePetName()
    {
        petName.text = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].petName;
    }
    public void SleepPet()
    {
        
        StartCoroutine(SleepCoroutine());
        
    }
    IEnumerator SleepCoroutine()
    {
        string exampleSleepTime = "05-45-00";
        string petWakeupTime = System.DateTime.Now.AddHours(int.Parse(exampleSleepTime.Substring(0, 2)))
            .AddMinutes(int.Parse(exampleSleepTime.Substring(3, 2)))
            .AddSeconds(int.Parse(exampleSleepTime.Substring(6, 2))).ToString("yyyy-MM-dd HH:mm:ss");
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].fullRestedTime = petWakeupTime;
        Debug.Log("Pet will wake up at: " + petWakeupTime);
        GameManager.instance.sleepTimeTemp = petWakeupTime;
        yield return DatabaseManager.instance.SavePlayerData(GameManager.instance.currentPlayerID);
        SceneManager.LoadScene("Sleep");

    }
    public void PlayWithPet()
    {
       isPaused = true;
       GameManager.instance.ShowGameStartUI();
       
    }
    
    public void FeedPet()
    {
        GameManager.instance.GetComponentInChildren<FoodMenuBehaviour>().OpenMenu();
        isPaused = true;
    }
    public void resumeFromGame()
    {
        isPaused = false;
        gameObject.GetComponent<PetBehaviour>().petUI.GetComponent<Canvas>().enabled = true;
    }
}


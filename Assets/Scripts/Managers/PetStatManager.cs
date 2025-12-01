using System.Collections;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel -= 0.1f;
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
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel = Mathf.Min(GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel + 1f, 10f);
        
    }
    public void FeedPet()
    {
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel = Mathf.Min(GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel + 1f, 10f);
    }
}


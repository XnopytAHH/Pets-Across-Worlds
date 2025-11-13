using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PetStatManager : MonoBehaviour
{
    public float foodLevel = 4f;
    public float moodLevel = 4f;
    public float energyLevel = 4f;
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
        foodSlider.value = foodLevel;
        moodSlider.value = moodLevel;
        energySlider.value = energyLevel;
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
            energyLevel -= 0.01f;
        }
    }
    public void UpdatePetName()
    {
        petName.text = GameManager.instance.UpdatePetStats(gameObject.transform.parent.name);
    }
    public void SleepPet()
    {
        string exampleSleepTime = "05-45-00";
        string petWakeupTime = System.DateTime.Now.AddHours(int.Parse(exampleSleepTime.Substring(0, 2)))
            .AddMinutes(int.Parse(exampleSleepTime.Substring(3, 2)))
            .AddSeconds(int.Parse(exampleSleepTime.Substring(6, 2))).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("Pet will wake up at: " + petWakeupTime);
        GameManager.instance.sleepTimeTemp = petWakeupTime;
        SceneManager.LoadScene("Sleep");
    }
    public void PlayWithPet()
    {
        moodLevel = Mathf.Min(moodLevel + 1f, 10f);
    }
    public void FeedPet()
    {
        foodLevel = Mathf.Min(foodLevel + 1f, 10f);
    }
}


/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Manages and updates the pet's statistics such as food, mood, and energy levels. Handles the UI above the pet's head.
*/
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
    /// <summary>
    /// Name of the pet displayed above its head.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI petName;
    /// <summary>
    /// Slider representing the pet's food level.
    /// </summary>
    [SerializeField]
    private Slider foodSlider;
    /// <summary>
    /// Slider representing the pet's mood level.
    /// </summary>
    [SerializeField]
    private Slider moodSlider;
    /// <summary>
    /// Slider representing the pet's energy level.
    /// </summary>
    [SerializeField]
    private Slider energySlider;
    
    /// <summary>
    /// Prefab for the food item.
    /// </summary>
    [SerializeField]
    private GameObject foodPrefab;
    /// <summary>
    /// Indicates whether the pet stats updates are paused.
    /// </summary>
    public bool isPaused = false;

    /// <summary>
    /// Updates the pet's stats UI every frame.
    /// </summary>
    void Update()
    {
        if (GameManager.instance.activePet == null) return;
        foodSlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel;
        moodSlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel;
        energySlider.value = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel;
        if (GameManager.instance.currentPlayerPets[GameManager.instance.activePet].energyLevel <= 0)
        {
            GameManager.instance.forcedSleep = true;
            // Trigger sleep state
            StartCoroutine(SleepCoroutine());
        }
    }
    /// <summary>
    /// Initializes the pet stat manager and starts the stat decrease coroutine.
    /// </summary>
    void Start()
    {
        StartCoroutine(DecreaseStatsOverTime());
    }
        
    /// <summary>
    /// Decreases the pet's energy level over time.
    /// </summary>
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
    /// <summary>
    /// Updates the pet's name displayed above its head.
    /// </summary>
    public void UpdatePetName()
    {
        petName.text = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].petName;
    }
    /// <summary>
    ///activates the sleep process for the pet, updating wake-up time and saving player data.
    /// </summary>
    public void SleepPet()
    {
        SoundManager.instance.buttonClick();
        StartCoroutine(SleepCoroutine());
        
    }
    /// <summary>
    /// Coroutine to handle pet sleep, updating wake-up time and saving player data.
    /// </summary>
    
    IEnumerator SleepCoroutine()
    {
        string SleepTime =  GameManager.instance.petSleepTimes[GameManager.instance.activePet];
        string petWakeupTime = System.DateTime.Now.AddHours(int.Parse(SleepTime.Substring(0, 2)))
            .AddMinutes(int.Parse(SleepTime.Substring(3, 2)))
            .AddSeconds(int.Parse(SleepTime.Substring(6, 2))).ToString("yyyy-MM-dd HH:mm:ss");
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].fullRestedTime = petWakeupTime;
        Debug.Log("Pet will wake up at: " + petWakeupTime);
        GameManager.instance.sleepTimeTemp = petWakeupTime;
        yield return DatabaseManager.instance.SavePlayerData(GameManager.instance.currentPlayerID);
        SceneManager.LoadScene("Sleep");

    }
    /// <summary>
    /// Handles the play action with the pet, showing the game start UI and hiding the to-do list.
    /// </summary>
    public void PlayWithPet()
    {
        SoundManager.instance.buttonClick();
       isPaused = true;
       GameManager.instance.ShowGameStartUI();
        GameManager.instance.HideTodoList();
       
    }
    /// <summary>
    /// Handles the feeding action with the pet, opening the food menu and hiding the to-do list.
    /// </summary>
    public void FeedPet()
    {
        SoundManager.instance.buttonClick();
        GameManager.instance.GetComponentInChildren<FoodMenuBehaviour>().OpenMenu();
        isPaused = true;
        GameManager.instance.HideTodoList();
    }
    /// <summary>
    /// Resumes pet activities from a paused state, re-enabling the pet UI and showing the to-do list.
    /// </summary>
    public void resumeFromGame()
    {
        isPaused = false;
        gameObject.GetComponent<PetBehaviour>().petUI.GetComponent<Canvas>().enabled = true;
        GameManager.instance.ShowTodoList();
    }
}


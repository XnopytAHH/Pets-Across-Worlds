/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Manages database interactions for loading and saving player data.
*/
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine.VFX;


public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager instance;
    /// <summary>
    /// Current player's ID.
    /// </summary>
    public string currentPlayerID;
    /// <summary>
    /// Dictionary of the current player's pets.
    /// </summary>
    public Dictionary<string, Pet> currentPlayerPets = new Dictionary<string, Pet>();
    
    /// <summary>
    /// Current player's name.
    /// </summary>
    public string currentPlayerName;
    /// <summary>
    /// Currently active pet's type.
    /// </summary>
    public string activePet = null;
    
    /// <summary>
    /// UI canvas for pet creation.
    /// </summary>
    [SerializeField]
    Canvas petCreationUI;
    /// <summary>
    /// UI GameObject for the to-do list.
    /// </summary>
    [SerializeField]
    GameObject todoListUI;
    /// <summary>
    /// Reference to the Firebase Realtime Database.
    /// </summary>
    DatabaseReference db;
    /// <summary>
    /// Temporary variable to hold the pet type during creation.
    /// </summary>
    string petTypeTemp;
    /// <summary>
    /// Temporary variable to hold the sleep time during scene changes
    /// </summary>
    public string sleepTimeTemp;
    /// <summary>
    /// Array of pet types available in the game.
    /// </summary>

    [SerializeField]
    public string[] petTypes;
    /// <summary>
    /// Array of favorite foods corresponding to each pet type.
    /// </summary>
    [SerializeField]
    private string[] favoriteFoods;
    /// <summary>
    /// Array of disliked foods corresponding to each pet type.
    /// </summary>
    [SerializeField]
    private string[] dislikedFoods;
    /// <summary>
    /// Array of sleep times corresponding to each pet type.
    /// </summary>
    [SerializeField]
    public string[] sleepTimes;
    /// <summary>
    /// Dictionary mapping pet types to their sleep times.
    /// </summary>
    public Dictionary<string, string> petSleepTimes;
    
    /// <summary>
    /// Dictionary mapping pet types to their food preferences.
    /// </summary>
    public Dictionary<string, string[]> petFoodPreferences;
    
    /// <summary>
    /// UI GameObject for the game start screen.
    /// </summary>
    [SerializeField]
    GameObject gameStartUI;
    /// <summary>
    /// UI GameObject for the game over screen.
    /// </summary>
    [SerializeField]
    GameObject gameOverUI;
    /// <summary>
    /// UI GameObject for the new highscore text.
    /// </summary>
    [SerializeField]
    GameObject NewHighscoreText;
    /// <summary>
    /// Flag indicating if the pet is forced to sleep or if the player triggered it.
    /// </summary>
    public bool forcedSleep = false;
    

    /// <summary>
    /// Initializes the GameManager singleton and sets up necessary data structures.
    /// </summary>
    void Awake()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        petCreationUI.enabled = false;
        petFoodPreferences = new Dictionary<string, string[]>();
        for (int i = 0; i < petTypes.Length; i++)
        {
            petFoodPreferences[petTypes[i]] = new string[] { favoriteFoods[i], dislikedFoods[i] };
        }
        petSleepTimes = new Dictionary<string, string>();
        for (int i = 0; i < petTypes.Length; i++)
        {
            petSleepTimes[petTypes[i]] = sleepTimes[i];
        }


    }
    /// <summary>
    /// Opens the pet creation UI for the specified pet type.
    /// </summary>
   
    public void CreateNewPet(string petType)
    {
        petCreationUI.enabled = true;
        petTypeTemp = petType;
    }
    /// <summary>
    /// Creates a new pet in the GameManager to be later saved to the database.
    /// Triggered by button on pet creation UI.
    /// Name is a bit misleading
    /// </summary>
    public void CreateNewPetDatabase()
    {
        SoundManager.instance.buttonClick();
        string petType = petTypeTemp;
        TMP_InputField petNameInput = GameObject.Find("PetNameInput").GetComponent<TMP_InputField>();
        string petName = petNameInput.text;
        if (petName == "")
        {
            Debug.Log("Pet name cannot be empty.");
            return;
        }
        else
        {
            Pet newPet = new Pet(petName, 5f, 5f, 10f, "", 0);
            GameManager.instance.currentPlayerPets.Add(petType, newPet);
            petCreationUI.enabled = false;
            GameManager.instance.activePet = petType;
            GameObject.Find("XR Origin (AR Rig)").GetComponent<ImageTracker>().petExists = true;
            GameObject.Find("XR Origin (AR Rig)").GetComponent<ImageTracker>().petActive = true;
            
        }
        ShowTodoList();

    }
    /// <summary>
    /// Checks if a pet with the given name already exists for the current player.
    /// </summary>
    public bool CheckPetExists(string petName)
    {
        if (petCreationUI.enabled == true)
        {
            Debug.Log("Pet creation in progress. Skipping existence check.");
            return false;
        }

        foreach (var pet in currentPlayerPets)
        {
            if (pet.Key == petName)
            {
                GameObject.Find("XR Origin (AR Rig)").GetComponent<ImageTracker>().petExists = true;
                return true;

            }
        }
        GameObject.Find("XR Origin (AR Rig)").GetComponent<ImageTracker>().petExists = false;
        return false;
    }
    /// <summary>
    /// Handles the pet resting state and updates relevant UI and game state.
    /// </summary>
    public string PetIsResting(string petName)
    {
        petCreationUI.enabled = false;
        GameManager.instance.activePet = null;
        string timeTillWake = (System.DateTime.Parse(GameManager.instance.currentPlayerPets[petName].fullRestedTime) - System.DateTime.Now).ToString();
        return timeTillWake;
    }
    /// <summary>
    /// Updates pet stats after resting based on the rest duration.
    /// </summary>
    public void UpdatePetStatsAfterRest(string petName)
    {
        GameManager.instance.currentPlayerPets[petName].moodLevel = Mathf.CeilToInt(GameManager.instance.currentPlayerPets[petName].moodLevel / 2);
        GameManager.instance.currentPlayerPets[petName].foodLevel = Mathf.CeilToInt(GameManager.instance.currentPlayerPets[petName].foodLevel / 2);
        if ((System.DateTime.Now - System.DateTime.Parse(GameManager.instance.currentPlayerPets[petName].fullRestedTime)).TotalDays > 0)
        {
            GameManager.instance.currentPlayerPets[petName].moodLevel -= 1f;
            GameManager.instance.currentPlayerPets[petName].foodLevel -= 1f;
            return;
        }
        GameManager.instance.currentPlayerPets[petName].energyLevel = 10f;
    }
    /// <summary>
    /// Displays the game start UI when play is selected. Shows the highscore.
    /// </summary>
    public void ShowGameStartUI()
    {
        gameStartUI.SetActive(true);
        gameStartUI.GetNamedChild("ScoreIndicator").GetComponent<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].highscore.ToString();
    }
    /// <summary>
    /// Displays the game over UI with score and mood increase details.
    /// </summary>
    public void ShowGameOverUI(int score)
    {
        gameOverUI.SetActive(true);
        int moodIncrease = Mathf.CeilToInt(score / 3);
        if (moodIncrease > 5)
        {
            moodIncrease = 5;
        }
        if (score > GameManager.instance.currentPlayerPets[GameManager.instance.activePet].highscore)
        {
            GameManager.instance.currentPlayerPets[GameManager.instance.activePet].highscore = score;
            NewHighscoreText.SetActive(true);
            moodIncrease += 3;
        }

        else
        {
            NewHighscoreText.SetActive(false);
        }
        gameOverUI.GetNamedChild("ScoreIndicator").GetComponent<TextMeshProUGUI>().text = score.ToString();

        gameOverUI.GetNamedChild("Details").GetComponent<TextMeshProUGUI>().text = "Your pet's mood increased by: " + moodIncrease.ToString();
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel += moodIncrease;

    }
    /// <summary>
    /// Hides the game over UI and resumes pet activities.
    /// </summary>
    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
        GameObject activePetObj = GameObject.FindGameObjectWithTag("ActivePet");
        activePetObj.GetComponent<PetStatManager>().resumeFromGame();
        StartCoroutine(activePetObj.GetComponent<PetBehaviour>().PlayStatUpVFX("mood"));
        SoundManager.instance.buttonClick();
    }
    /// <summary>
    /// Hides the game start UI and shows the to-do list.
    /// </summary>
    public void HideGameStartUI()
    {
        gameStartUI.SetActive(false);
        ShowTodoList();
    }
    /// <summary>
    /// Starts the game when the player presses the start button.
    /// </summary>
    public void startGame()
    {
        SoundManager.instance.buttonClick();
        GameObject activePetObj = GameObject.FindGameObjectWithTag("ActivePet");
        activePetObj.GetComponent<Play>().StartGame();
        activePetObj.GetComponent<PetBehaviour>().StartPlaying();
        HideGameStartUI();
        HideTodoList();
    }
    /// <summary>
    /// Hides the to-do list UI.
    /// </summary>
    public void HideTodoList()
    {
        todoListUI.SetActive(false);
    }
    /// <summary>
    /// Shows the to-do list UI.
    /// </summary>
    public void ShowTodoList()
    {
        todoListUI.SetActive(true);
    }
    
}

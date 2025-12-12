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


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string currentPlayerID;
    public Dictionary<string, Pet> currentPlayerPets = new Dictionary<string, Pet>();
    public string currentPlayerName;
    public string activePet = null;
    [SerializeField]
    Canvas petCreationUI;
    [SerializeField]
    GameObject todoListUI;
    DatabaseReference db;
    string petTypeTemp;
    public string sleepTimeTemp;

    [SerializeField]
    public string[] petTypes;
    [SerializeField]
    private string[] favoriteFoods;
    [SerializeField]
    private string[] dislikedFoods;
    [SerializeField]
    public string[] sleepTimes;
    public Dictionary<string, string> petSleepTimes;
    public Dictionary<string, string[]> petFoodPreferences;
    [SerializeField]
    GameObject gameStartUI;
    [SerializeField]
    GameObject gameOverUI;
    [SerializeField]
    GameObject NewHighscoreText;
    public bool forcedSleep = false;

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
    public void CreateNewPet(string petType)
    {
        petCreationUI.enabled = true;
        petTypeTemp = petType;
    }
    public void CreateNewPetDatabase()
    {
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

    }
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
    public string PetIsResting(string petName)
    {
        petCreationUI.enabled = false;
        GameManager.instance.activePet = null;
        string timeTillWake = (System.DateTime.Parse(GameManager.instance.currentPlayerPets[petName].fullRestedTime) - System.DateTime.Now).ToString();
        return timeTillWake;
    }
    public void UpdatePetStatsAfterRest(string petName)
    {
        GameManager.instance.currentPlayerPets[petName].moodLevel = Mathf.CeilToInt(GameManager.instance.currentPlayerPets[petName].moodLevel/2);
        GameManager.instance.currentPlayerPets[petName].foodLevel = Mathf.CeilToInt(GameManager.instance.currentPlayerPets[petName].foodLevel/2);
        if ((System.DateTime.Now - System.DateTime.Parse(GameManager.instance.currentPlayerPets[petName].fullRestedTime)).TotalDays > 0)
        {
            GameManager.instance.currentPlayerPets[petName].moodLevel -=1f;
            GameManager.instance.currentPlayerPets[petName].foodLevel -=1f;
            return;
        }
        GameManager.instance.currentPlayerPets[petName].energyLevel = 10f;
    }
    public void ShowGameStartUI()
    {
        gameStartUI.SetActive(true);
        gameStartUI.GetNamedChild("ScoreIndicator").GetComponent<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[GameManager.instance.activePet].highscore.ToString();
    }
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
            moodIncrease +=3;
        }
        
        else
        {
            NewHighscoreText.SetActive(false);
        }
        gameOverUI.GetNamedChild("ScoreIndicator").GetComponent<TextMeshProUGUI>().text = score.ToString();
        
        gameOverUI.GetNamedChild("Details").GetComponent<TextMeshProUGUI>().text = "Your pet's mood increased by: " + moodIncrease.ToString();
        GameManager.instance.currentPlayerPets[GameManager.instance.activePet].moodLevel += moodIncrease;

    }
    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
        GameObject activePetObj = GameObject.FindGameObjectWithTag("ActivePet");
        activePetObj.GetComponent<PetStatManager>().resumeFromGame();
    }
    public void HideGameStartUI()
    {
        gameStartUI.SetActive(false);
        ShowTodoList();
    }
    public void startGame()
    {
        GameObject activePetObj = GameObject.FindGameObjectWithTag("ActivePet");
       activePetObj.GetComponent<Play>().StartGame();
       activePetObj.GetComponent<PetBehaviour>().StartPlaying();
       HideGameStartUI();
       HideTodoList();
    }
    public void HideTodoList()
    {
        todoListUI.SetActive(false);
    }
    public void ShowTodoList()
    {
        todoListUI.SetActive(true);
    }
    
}   

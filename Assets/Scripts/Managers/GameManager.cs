using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string currentPlayerID;
    public Dictionary<string, Pet> currentPlayerPets = new Dictionary<string, Pet>();
    public string currentPlayerName;
    public string activePet = null;
    [SerializeField]
    Canvas petCreationUI;
    DatabaseReference db;
    string petTypeTemp;
    public string sleepTimeTemp;
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
            Pet newPet = new Pet(petName, 10f, 10f, 10f, "");
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
    
}

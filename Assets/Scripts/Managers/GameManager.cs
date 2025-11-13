using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string currentPlayerID;
    public string[] trackedImages;
    [SerializeField]
    Canvas petCreationUI;
    DatabaseReference db;
    string petTypeTemp;
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
            string json = JsonUtility.ToJson(newPet);
            db.Child("players").Child(currentPlayerID).Child("pets").Child(petType).SetRawJsonValueAsync(json);
            petCreationUI.enabled = false;
        }
        
    }
    public bool CheckPetExists(string petName)
    {
        bool petExists = false;

        db.Child("players").Child(currentPlayerID).Child("pets").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve pets from database.");
                petExists = false;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot petSnapshot in snapshot.Children)
                {
                    if (petSnapshot.Key == petName)
                    {
                        petExists = true;
                    }
                }
            }
        });
        return petExists;
    }
    public string UpdatePetStats(string petName)
    {
        string updatedName = "";
        db.Child("players").Child(currentPlayerID).Child("pets").Child(petName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve pet stats from database.");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string petJson = snapshot.GetRawJsonValue();
                    Debug.Log(petJson);
                    Pet pet = JsonUtility.FromJson<Pet>(petJson);
                    updatedName = pet.petName;
                }
            }
        });
        Debug.Log("Updated Pet Name: " + updatedName);
        return updatedName;
    }
}

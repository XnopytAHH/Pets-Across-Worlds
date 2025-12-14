/*
* Author: Lim En Xu Jayson
* Date: 17 November 2025
* Description: Manages database interactions for loading and saving player data.
*/
using System.Data.Common;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the DatabaseManager.
    /// </summary>
    public static DatabaseManager instance;
    /// <summary>
    /// Reference to the Firebase Realtime Database.
    /// </summary>
    DatabaseReference db;
    
    /// <summary>
    /// Initializes the DatabaseManager and sets up the Firebase database reference.
    /// </summary>
    void Start()
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
    }

    /// <summary>
    /// Loads player data from the Firebase Realtime Database and loads the data to GameManager.
    /// </summary>
    public async Task LoadPlayerData(string playerId)
    {
        try
        {
            var snapshot = await db.Child("players").Child(playerId).GetValueAsync();
            if (!snapshot.Exists)
            {
                Debug.LogError("Player data does not exist.");
                return;
            }
            Debug.Log("Player data loaded successfully.");
            GameManager.instance.currentPlayerName = snapshot.Child("username").Value.ToString();
            foreach (var petSnapshot in snapshot.Child("pets").Children)
            {
                
                GameManager.instance.currentPlayerPets.Add(
                    petSnapshot.Key,
                    new Pet(
                        petSnapshot.Child("petName").Value.ToString(),
                        float.Parse(petSnapshot.Child("foodLevel").Value.ToString()),
                        float.Parse(petSnapshot.Child("moodLevel").Value.ToString()),
                        float.Parse(petSnapshot.Child("energyLevel").Value.ToString()),
                        petSnapshot.Child("fullRestedTime").Value.ToString(),
                        int.Parse(petSnapshot.Child("highscore").Value.ToString())
                    )
                );
            }
        }
        
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load player data: " + e.Message);
        }
    }

    /// <summary>
    /// Saves player data to the Firebase Realtime Database from GameManager.
    /// </summary>
    public async Task SavePlayerData(string playerId)
    {
        try
        {
            foreach (var petEntry in GameManager.instance.currentPlayerPets)
            {
                string petType = petEntry.Key;
                Pet pet = petEntry.Value;
                string json = JsonUtility.ToJson(pet);
                await db.Child("players").Child(playerId).Child("pets").Child(petType).SetRawJsonValueAsync(json);
            }
            Debug.Log("Player data saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save player data: " + e.Message);
        }
    }
}


using System.Data.Common;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    DatabaseReference db;
    // Use this for initialization if needed
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
                new Pet(
                    petSnapshot.Child("petName").Value.ToString(),
                    float.Parse(petSnapshot.Child("foodLevel").Value.ToString()),
                    float.Parse(petSnapshot.Child("moodLevel").Value.ToString()),
                    float.Parse(petSnapshot.Child("energyLevel").Value.ToString()),
                    petSnapshot.Child("fullRestedTime").Value.ToString(),
                    int.Parse(petSnapshot.Child("highscore").Value.ToString())
                );
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

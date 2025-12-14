/*
* Author: Lim En Xu Jayson
* Date: 13 November 2025
* Description: Serializable data model representing a pet and its stats.
*/
[System.Serializable]
public class Pet
{
    /// <summary>
    /// The display name of the pet.
    /// </summary>
    public string petName;
    /// <summary>
    /// Current food level of the pet.
    /// </summary>
    public float foodLevel;
    /// <summary>
    /// Current mood level of the pet.
    /// </summary>
    public float moodLevel;
    /// <summary>
    /// Current energy level of the pet.
    /// </summary>
    public float energyLevel;
    /// <summary>
    /// The timestamp when the pet will be fully rested.
    /// </summary>
    public string fullRestedTime;
    /// <summary>
    /// Highest score achieved in mini-games.
    /// </summary>
    public int highscore;

    /// <summary>
    /// Constructs a new pet with initial stats and metadata.
    /// </summary>
    public Pet(string PetName, float FoodLevel, float MoodLevel, float EnergyLevel, string FullRestedTime, int Highscore)
    {
        petName = PetName;
        foodLevel = FoodLevel;
        moodLevel = MoodLevel;
        energyLevel = EnergyLevel;
        fullRestedTime = FullRestedTime;
        highscore = Highscore;
    }
}
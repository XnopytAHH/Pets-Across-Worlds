[System.Serializable]
public class Pet
{
    public string petName;
    public float foodLevel;
    public float moodLevel;
    public float energyLevel;
    public string fullRestedTime;
    public int highscore;

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
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PetStatManager : MonoBehaviour
{
    public float foodLevel = 10f;
    public float moodLevel = 10f;
    public float energyLevel = 10f;
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
}


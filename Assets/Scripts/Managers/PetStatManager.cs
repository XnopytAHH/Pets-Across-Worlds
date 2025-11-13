using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetStatManager : MonoBehaviour
{
    public float foodLevel = 10f;
    public float moodLevel = 10f;
    public float energyLevel = 10f;
    [SerializeField]
    private TextMeshProUGUI petName;
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
    public void UpdatePetName()
    {
        petName.text = GameManager.instance.UpdatePetStats(gameObject.transform.parent.name);
    }
}


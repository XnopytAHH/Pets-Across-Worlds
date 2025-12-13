using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
public class PetBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject petModel;    
    public bool petAwake = true;
    [SerializeField]
    public GameObject petUI;
    [SerializeField]
    private GameObject restUI;
    [SerializeField]
    VisualEffect statUpVFX;
    [SerializeField]
    Texture2D moodStatUpTexture;
    [SerializeField]
    Texture2D foodStatUpTexture;
    [SerializeField]
    GameObject exclaimPrefab;
    

    void Start()
    {
        statUpVFX.Stop();
    }
    void Update()
    {
        if(!petAwake)
        {
            petModel.SetActive(false);
            petUI.SetActive(false);
            restUI.SetActive(true);
        }
        else
        {
            petModel.SetActive(true);
            petUI.SetActive(true);
            restUI.SetActive(false);

        }  
        Vector3 offsetPosition = GameObject.Find("Location Ref").GetComponent<PetLocationRef>().GetPetPosition(GameObject.Find("Movement Plane").transform.localScale.x);
        gameObject.transform.rotation = GameObject.Find("Location Ref").GetComponent<PetLocationRef>().GetRotation();
        offsetPosition = GameObject.Find("Movement Plane").transform.position + new Vector3(offsetPosition.x, 0, offsetPosition.z);
        offsetPosition.y = gameObject.transform.position.y;
        gameObject.transform.position = offsetPosition;
    }
    public void StartPlaying()
    {
        Debug.Log("Pet Started Playing");
        petUI.GetComponent<Canvas>().enabled = false;
        StartCoroutine(GameObject.Find("Location Ref").GetComponent<PetLocationRef>().SwitchStates("Playing"));
    }

    public void UpdateRestUI(string timeTillWake)
    {
        restUI.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[gameObject.transform.parent.name].petName + " is asleep! They will wake up in " + timeTillWake.Substring(0, 8);
        
    }
    public IEnumerator PlayStatUpVFX(string statName)
    {
        if (statName == "mood")
        {
            statUpVFX.SetTexture("Stat", moodStatUpTexture);
        }
        else if (statName == "food")
        {
            statUpVFX.SetTexture("Stat", foodStatUpTexture);
        }
        
        statUpVFX.Play();
        yield return new WaitForSeconds(0.5f);
        statUpVFX.Stop();
        yield return null;
    }
    public void LookAtFood(GameObject target)
    {
        Vector3 directionToFood = target.transform.position - transform.position;
        directionToFood.y = 0; // Keep only the horizontal direction
        Quaternion foodRotation = Quaternion.LookRotation(directionToFood);
        StartCoroutine(GameObject.Find("Location Ref").GetComponent<PetLocationRef>().FaceFood(foodRotation));
        Instantiate(exclaimPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
    }
}

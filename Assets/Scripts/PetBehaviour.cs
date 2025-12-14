/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Controls pet visuals, UI state, reactions, and stat-up VFX.
*/
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
public class PetBehaviour : MonoBehaviour
{
    /// <summary>
    /// Reference to the pet's 3D model.
    /// </summary>
    [SerializeField]
    GameObject petModel;    
    /// <summary>
    /// Whether the pet is awake.
    /// </summary>
    public bool petAwake = true;
    /// <summary>
    /// UI canvas and elements associated with the pet.
    /// </summary>
    [SerializeField]
    public GameObject petUI;
    /// <summary>
    /// UI shown when the pet is resting.
    /// </summary>
    [SerializeField]
    private GameObject restUI;
    /// <summary>
    /// VFX graph used to show stat increases.
    /// </summary>
    [SerializeField]
    VisualEffect statUpVFX;
    /// <summary>
    /// Texture used when mood stat increases.
    /// </summary>
    [SerializeField]
    Texture2D moodStatUpTexture;
    /// <summary>
    /// Texture used when food stat increases.
    /// </summary>
    [SerializeField]
    Texture2D foodStatUpTexture;
    /// <summary>
    /// Exclamation indicator prefab spawned on attention events.
    /// </summary>
    [SerializeField]
    GameObject exclaimPrefab;
    

    /// <summary>
    /// Initializes visual effects.
    /// </summary>
    void Start()
    {
        statUpVFX.Stop();
    }
    /// <summary>
    /// Updates pet activation state and positions relative to AR content.
    /// </summary>
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
    /// <summary>
    /// Hides pet UI and enters playing state.
    /// </summary>
    public void StartPlaying()
    {
        Debug.Log("Pet Started Playing");
        petUI.GetComponent<Canvas>().enabled = false;
        StartCoroutine(GameObject.Find("Location Ref").GetComponent<PetLocationRef>().SwitchStates("Playing"));
    }

    /// <summary>
    /// Updates the sleep UI with time remaining until wake.
    /// </summary>
    public void UpdateRestUI(string timeTillWake)
    {
        restUI.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[gameObject.transform.parent.name].petName + " is asleep! They will wake up in " + timeTillWake.Substring(0, 8);
        
    }
    /// <summary>
    /// Plays stat-up VFX for the specified stat.
    /// </summary>
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
    /// <summary>
    /// Rotates pet toward food and spawns an exclaim indicator.
    /// </summary>
    public void LookAtFood(GameObject target)
    {
        Vector3 directionToFood = target.transform.position - transform.position;
        directionToFood.y = 0; // Keep only the horizontal direction
        Quaternion foodRotation = Quaternion.LookRotation(directionToFood);
        StartCoroutine(GameObject.Find("Location Ref").GetComponent<PetLocationRef>().FaceFood(foodRotation));
        Instantiate(exclaimPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
    }
}

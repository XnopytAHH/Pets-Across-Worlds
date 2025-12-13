using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
public class PetBehaviour : MonoBehaviour
{
    
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
    

    void Start()
    {
        statUpVFX.Stop();
    }
    void Update()
    {
        if(!petAwake)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            petUI.SetActive(false);
            restUI.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            petUI.SetActive(true);
            restUI.SetActive(false);

        }  
        Vector3 offsetPosition = GameObject.Find("Location Ref").GetComponent<PetLocationRef>().GetPetPosition(GameObject.Find("Movement Plane").transform.localScale.x);
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
    
}

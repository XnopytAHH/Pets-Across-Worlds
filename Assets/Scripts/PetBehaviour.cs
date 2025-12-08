using UnityEngine;

using System.Collections;
using UnityEngine.AI;
using Unity.XR.CoreUtils;
using TMPro;
public class PetBehaviour : MonoBehaviour
{
    
    public bool petAwake = true;
    [SerializeField]
    private GameObject petUI;
    [SerializeField]
    private GameObject restUI;

    void Start()
    {
        
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

    public void UpdateRestUI(string timeTillWake)
    {
        restUI.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.instance.currentPlayerPets[gameObject.transform.parent.name].petName + " is asleep! They will wake up in " + timeTillWake.Substring(0, 8);
        
    }

    
}

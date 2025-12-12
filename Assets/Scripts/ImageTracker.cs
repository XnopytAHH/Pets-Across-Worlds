using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using System.Threading.Tasks;
using Unity.XR.CoreUtils;
using System.Collections;

public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    public bool petExists = false;
    public bool petActive = false;
    public string currentGameState = "WaitingForPet";
    private void Start()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.AddListener(OnImageChanged);
            SetupPrefabs();
        }
        
    }
    

    void SetupPrefabs()
    {
        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

    void OnImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        foreach (KeyValuePair<TrackableId, ARTrackedImage> lostObj in eventArgs.removed)
        {
            UpdateImage(lostObj.Value);
        }
    }

    public void UpdateImage(ARTrackedImage trackedImage)
    {
        if (trackedImage != null)
        {

            if (trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None)
            {
                Debug.Log("Tracking lost for image: " + trackedImage.referenceImage.name);
            }
            else if (trackedImage.trackingState == TrackingState.Tracking)
            {
        
                if (!petActive)
                {
                    GameManager.instance.CheckPetExists(trackedImage.referenceImage.name);
                    if (trackedImage.referenceImage.name != GameManager.instance.activePet && (GameManager.instance.activePet == null || GameManager.instance.activePet == ""))
                    {
                        Debug.Log("exists value: " + petExists);
                        if (petExists == false)
                        {
                            GameManager.instance.CreateNewPet(trackedImage.referenceImage.name);
                            
                        }
                        else if (petExists == true)
                        {
                            if (GameManager.instance.currentPlayerPets[trackedImage.referenceImage.name].fullRestedTime != "")
                            {
                                if (System.DateTime.Parse(GameManager.instance.currentPlayerPets[trackedImage.referenceImage.name].fullRestedTime) > System.DateTime.Now)
                                {
                                    spawnedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                                    spawnedPrefabs[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                                    spawnedPrefabs[trackedImage.referenceImage.name].SetActive(true);
                                    spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetBehaviour>().petAwake = false;
                                    string timeTillWake = GameManager.instance.PetIsResting(trackedImage.referenceImage.name);
                                    spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetBehaviour>().UpdateRestUI(timeTillWake);
                                    return;
                                }
                                else
                                {
                                    GameManager.instance.UpdatePetStatsAfterRest(trackedImage.referenceImage.name);
                                    GameManager.instance.currentPlayerPets[trackedImage.referenceImage.name].fullRestedTime = "";
                                    
                                }
                            }
                            GameManager.instance.activePet = trackedImage.referenceImage.name;
                            spawnedPrefabs[trackedImage.referenceImage.name].GetNamedChild("Capsule").tag = "ActivePet";
                            GameManager.instance.ShowTodoList();
                            petActive = true;
                            Debug.Log("PET ACTIVE SET TO TRUE");
                        }
                        else
                        {
                            Debug.Log("Pet existence unknown. Cannot proceed.");
                            return;
                        }
                    }
                }
                else
                {
                    if (trackedImage.referenceImage.name == GameManager.instance.activePet)
                    {
                        
                        spawnedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                        spawnedPrefabs[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                        spawnedPrefabs[trackedImage.referenceImage.name].SetActive(true);
                        spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetBehaviour>().petAwake = true;
                        spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetStatManager>().UpdatePetName();
                    }
                    else
                    {
                        
                        return;
                    }
                }


            }
            //Enable the associated content

        }
    }
}

/*
* Author: Lim En Xu Jayson (Based on original by Elyas Chua-Aziz)
* Date: 9 November 2025
* Description: Tracks AR reference images and manages spawning/updating pet prefabs.
*/
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
    /// <summary>
    /// ARTrackedImageManager responsible for tracking reference images.
    /// </summary>
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    /// <summary>
    /// Prefabs that can be spawned when their corresponding image is tracked.
    /// </summary>
    [SerializeField]
    private GameObject[] placeablePrefabs;

    /// <summary>
    /// Runtime instances of spawned prefabs keyed by reference image name.
    /// </summary>
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    /// <summary>
    /// Whether a pet already exists in the player's data.
    /// </summary>
    public bool petExists = false;
    /// <summary>
    /// Whether a pet is currently active in the scene.
    /// </summary>
    public bool petActive = false;
    /// <summary>
    /// Current game state used by the tracker.
    /// </summary>
    public string currentGameState = "WaitingForPet";
    /// <summary>
    /// UI text shown while scanning for images.
    /// </summary>
    [SerializeField]
    public GameObject scanText;
    /// <summary>
    /// Initializes tracking and prefab setup.
    /// </summary>
    private void Start()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackablesChanged.AddListener(OnImageChanged);
            SetupPrefabs();
        }
        
    }
    

    /// <summary>
    /// Instantiates and registers all placeable prefabs.
    /// </summary>
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

    /// <summary>
    /// Handles tracked image add/update/remove events.
    /// </summary>
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

    /// <summary>
    /// Updates scene content in response to tracked image state changes.
    /// </summary>
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
                            scanText.SetActive(false);
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
                            scanText.SetActive(false);
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
                         
                        scanText.SetActive(false);
                        spawnedPrefabs[trackedImage.referenceImage.name].GetNamedChild("Capsule").tag = "ActivePet";
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

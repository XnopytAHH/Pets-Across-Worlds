using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using System.Threading.Tasks;

public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

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

    async Task UpdateImage(ARTrackedImage trackedImage)
    {
        if(trackedImage != null)
        {
            
            if (trackedImage.trackingState == TrackingState.Limited || trackedImage.trackingState == TrackingState.None)
            {
                //Disable the associated content
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            }
            else if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Debug.Log("Tracked Image: " + trackedImage.referenceImage.name);
                Debug.Log("activePet: " + GameManager.instance.activePet);
                
                if (trackedImage.referenceImage.name != GameManager.instance.activePet && (GameManager.instance.activePet == null || GameManager.instance.activePet == ""))
                {
                    Debug.Log("No active pet. Creating or selecting pet for tracked image: " + trackedImage.referenceImage.name);
                    bool petExists = await GameManager.instance.CheckPetExists(trackedImage.referenceImage.name);
                    if (!petExists)
                    {
                    Debug.Log("Pet does not exist. Creating new pet: " + trackedImage.referenceImage.name);
                    GameManager.instance.CreateNewPet(trackedImage.referenceImage.name);
                    }
                    else
                    {
                    Debug.Log("Pet exists. Setting active pet to: " + trackedImage.referenceImage.name);
                        GameManager.instance.activePet = trackedImage.referenceImage.name;
                    }
                }
                else if (trackedImage.referenceImage.name == GameManager.instance.activePet)
                {
                    Debug.Log("Active pet matched with tracked image.");
                    spawnedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                    spawnedPrefabs[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                    spawnedPrefabs[trackedImage.referenceImage.name].SetActive(true);
                    spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetStatManager>().UpdatePetName();
                }
                else
                {
                    Debug.Log("Tracked image does not match active pet. Ignoring.");
                    return;
                }
                
            }
            //Enable the associated content
                    
        }
    }
}

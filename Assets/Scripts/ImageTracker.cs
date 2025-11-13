using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;

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

    void UpdateImage(ARTrackedImage trackedImage)
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
                if (!GameManager.instance.trackedImages.Contains(trackedImage.referenceImage.name) && !GameManager.instance.CheckPetExists(trackedImage.referenceImage.name))
                {

                    GameManager.instance.trackedImages = GameManager.instance.trackedImages.Append(trackedImage.referenceImage.name).ToArray();
                    GameManager.instance.CreateNewPet(trackedImage.referenceImage.name);
                }
                //Enable the associated content
                spawnedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                spawnedPrefabs[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(true);
                spawnedPrefabs[trackedImage.referenceImage.name].GetComponentInChildren<PetStatManager>().UpdatePetName();
            }
        }
    }
}

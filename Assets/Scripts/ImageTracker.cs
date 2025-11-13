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
    public bool? petExists = null;
    public bool petActive = false;

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
                //Disable the associated content
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
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
                            GameManager.instance.activePet = trackedImage.referenceImage.name;
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


            }
            //Enable the associated content

        }
    }
}

using UnityEngine;
using System.Collections.Generic;
public class FoodMenuBehaviour : MonoBehaviour
{   

    [SerializeField]
    private string[] foodNameList;
    [SerializeField]
    private GameObject[] foodPrefabList;
    public Dictionary<string, GameObject> foodPrefabs;
    Canvas foodMenuCanvas;
    void Start()
    {
        foodMenuCanvas = gameObject.GetComponent<Canvas>();
        CloseMenu();
        foodPrefabs = new Dictionary<string, GameObject>();
        for (int i = 0; i < foodNameList.Length; i++)
        {
            foodPrefabs.Add(foodNameList[i], foodPrefabList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnFood(string foodName)
    {
        GameObject foodObject = Instantiate(foodPrefabs[foodName], GameObject.Find(GameManager.instance.activePet).transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity );
        CloseMenu();
    }
    public void CloseMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }
    public void OpenMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
    }
}

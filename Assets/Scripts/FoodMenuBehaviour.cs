/*
* Author: Lim En Xu Jayson
* Date: 8 Decem 2025
* Description: Displays food menu UI, spawns selected food, and toggles todo UI.
*/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class FoodMenuBehaviour : MonoBehaviour
{   

    [SerializeField]
    private string[] foodNameList;
    [SerializeField]
    private GameObject[] foodPrefabList;
    public Dictionary<string, GameObject> foodPrefabs;
    Canvas foodMenuCanvas;
    /// <summary>
    /// Initializes menu, builds food prefab dictionary, and hides UI.
    /// </summary>
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

    /// <summary>
    /// Spawns selected food near active pet and closes the menu.
    /// </summary>
    public void SpawnFood(string foodName)
    {
        SoundManager.instance.buttonClick();
        GameObject foodObject = Instantiate(foodPrefabs[foodName], GameObject.Find(GameManager.instance.activePet).transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity );
        
        GameObject.FindGameObjectWithTag("ActivePet").GetComponent<PetBehaviour>().LookAtFood(foodObject);
        CloseMenu();
    }
    /// <summary>
    /// Hides the food menu and (in MainMenu) re-shows the todo UI.
    /// </summary>
    public void CloseMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            GameManager.instance.ShowTodoList();
        }
        
    }
    /// <summary>
    /// Shows the food menu and hides the todo UI.
    /// </summary>
    public void OpenMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        GameManager.instance.HideTodoList();
    }
}

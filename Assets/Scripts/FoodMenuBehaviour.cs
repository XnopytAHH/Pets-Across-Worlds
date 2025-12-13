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
        SoundManager.instance.buttonClick();
        GameObject foodObject = Instantiate(foodPrefabs[foodName], GameObject.Find(GameManager.instance.activePet).transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity );
        
        GameObject.FindGameObjectWithTag("ActivePet").GetComponent<PetBehaviour>().LookAtFood(foodObject);
        CloseMenu();
    }
    public void CloseMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            GameManager.instance.ShowTodoList();
        }
        
    }
    public void OpenMenu()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        GameManager.instance.HideTodoList();
    }
}

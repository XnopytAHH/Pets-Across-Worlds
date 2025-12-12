using UnityEngine;

public class TodoManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] todoItems;
    [SerializeField]
    GameObject todo1;
    [SerializeField]
    GameObject todo2;
    void Update()
    {
        int successful = 0;
        for (int i = 0; i < todoItems.Length; i++)
        {
            
            if (todoItems[i].GetComponent<TodoElement>().IsCompleted != true)
            {
                break;
            }
            else
            {
                successful +=1;
            }
            
        }
        if (successful == todoItems.Length)
        {
            todo1.SetActive(false);
            todo2.SetActive(true);
        }
        else
        {
            todo1.SetActive(true);
            todo2.SetActive(false);
        }
    }
}

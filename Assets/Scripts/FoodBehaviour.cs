using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    Vector2 petScreenPosition;
    Vector2 foodScreenPosition;
    
    void Update()
    {   
        if (GameManager.instance.activePet != null)
        {
            petScreenPosition = Camera.main.WorldToScreenPoint(
                GameObject.Find(GameManager.instance.activePet).GetNamedChild("Capsule").transform.position);
            foodScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            float distance = Vector2.Distance(petScreenPosition, foodScreenPosition);
            float foodAmount= 0f;
            if (distance < 50f)
            {
                int foodPreference = checkFavoriteFood(gameObject.name.Replace("(Clone)", "").Trim());
                if (foodPreference == 1) // favorite food
                {
                    foodAmount = 3f;
                }
                else if (foodPreference == 0) // normal food
                {
                    foodAmount = 1f;
                }
                else // disliked food
                {
                    foodAmount = 0.5f;
                }

                
                GameManager.instance.currentPlayerPets[GameManager.instance.activePet].foodLevel += foodAmount;
                Destroy(gameObject);
            }
        }
        
    }
    int checkFavoriteFood(string foodName)
    {
        string petType = GameManager.instance.activePet;
        string petFavorite = GameManager.instance.petFoodPreferences[petType][0];
        string petDisliked = GameManager.instance.petFoodPreferences[petType][1];
        if (foodName == petFavorite)
        {
            return 1; // favorite food
        }
        else if (foodName == petDisliked)
        {
            return -1; // disliked food
        }
        else
        {
            return 0; // normal food
        }
    }
    }

/*
* Author: Lim En Xu Jayson
* Date: 8 December 2025
* Description: Handles food items approaching the pet and applying food effects.
*/
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    Vector2 petScreenPosition;
    Vector2 foodScreenPosition;
    
    /// <summary>
    /// Checks proximity to pet and applies food effects.
    /// </summary>
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
                
                SoundManager.instance.petEating();
                StartCoroutine(GameObject.FindGameObjectWithTag("ActivePet").GetComponent<PetBehaviour>().PlayStatUpVFX("food"));
                Destroy(gameObject);
            }
        }
        
    }
    /// <summary>
    /// Returns 1 if favorite, 0 if neutral, -1 if disliked.
    /// </summary>
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

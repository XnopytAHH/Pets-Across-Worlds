/*
* Author: Lim En Xu Jayson
* Date: 9 November 2025
* Description: Handles login/register UI flow and Firebase auth integration.
*/
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.XR.CoreUtils;
using System.Collections.Generic;

public class LoginAndRegister : MonoBehaviour
{
    [SerializeField]
    GameObject loginUI;
    [SerializeField]
    GameObject registerUI;
    [SerializeField]
    GameObject mainMenuUI;
    [SerializeField]
    TextMeshProUGUI loginErrorText;
    [SerializeField]
    TextMeshProUGUI registerErrorText;
    private bool isLoggingIn = true;
    /// <summary>
    /// Initializes UI visibility depending on login state and hides todo UI.
    /// </summary>
    public void Start()
    {
        if (GameManager.instance.currentPlayerID != null && GameManager.instance.currentPlayerID != "")
        {
            loginUI.SetActive(false);
            registerUI.SetActive(false);
            mainMenuUI.SetActive(true);
            mainMenuUI.GetNamedChild("Username").GetComponent<TextMeshProUGUI>().text = GameManager.instance.currentPlayerName;
        }
        else
        {
            loginUI.SetActive(true);
            registerUI.SetActive(false);
            mainMenuUI.SetActive(false);
        }
        GameManager.instance.HideTodoList();
            
        
    }
    /// <summary>
    /// Toggles between Login and Register UIs.
    /// </summary>
    public void changeMenu()
    {
        SoundManager.instance.buttonClick();
        isLoggingIn = !isLoggingIn;
        loginUI.SetActive(isLoggingIn);
        registerUI.SetActive(!isLoggingIn);
    }
    /// <summary>
    /// Performs login or registration, shows errors, and starts wait coroutine.
    /// </summary>
    public void LogInOrRegister()
    {
        SoundManager.instance.buttonClick();
        TMP_InputField emailInputField = GameObject.FindGameObjectWithTag("EmailField").GetComponent<TMP_InputField>();
        TMP_InputField passwordInputField = GameObject.FindGameObjectWithTag("PasswordField").GetComponent<TMP_InputField>();
        
        
            if (isLoggingIn)
            {
                TextMeshProUGUI errorText =  loginErrorText;
                FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Login was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    var exception = task.Exception.GetBaseException() as Firebase.FirebaseException;
                    var errorCode = (Firebase.Auth.AuthError)exception.ErrorCode;
                    switch (errorCode)
                    {
                        case Firebase.Auth.AuthError.MissingEmail:
                            errorText.text = ("Please enter your email address.");
                            break;
                        case Firebase.Auth.AuthError.MissingPassword:
                            errorText.text = ("Please enter your password.");
                            break;
                        case Firebase.Auth.AuthError.InvalidEmail:
                            errorText.text = ("Please enter a valid email address.");
                            break;
                        case Firebase.Auth.AuthError.WrongPassword:
                            errorText.text = ("The password is incorrect. Please try again.");
                            break;
                        case Firebase.Auth.AuthError.UserNotFound:
                            errorText.text = ("There is no account with this email. Please register first.");
                            break;
                        case Firebase.Auth.AuthError.WebInternalError:
                            errorText.text = ("This account has been disabled. Please contact support.");
                            break;
                        default:
                            Debug.LogError("Login encountered an error: " + exception.Message);
                            break;
                    }
                    return;
                }
                GameManager.instance.currentPlayerID = task.Result.User.UserId;
                
            });
            }
            else
            {
                TextMeshProUGUI errorText =  registerErrorText;
                if (GameObject.Find("UsernameInput").GetComponent<TMP_InputField>().text == "")
                {
                    errorText.text = ("Please enter a username.");
                    return;
                }
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text)
                .ContinueWithOnMainThread(task =>
                {
                    
                    if (task.IsFaulted)
                    {
                        var exception = task.Exception.GetBaseException() as Firebase.FirebaseException;
                        var errorCode = (Firebase.Auth.AuthError)exception.ErrorCode;
                        switch (errorCode)
                        {
                            case Firebase.Auth.AuthError.MissingEmail:
                                errorText.text = ("Please enter your email address.");
                                break;
                            case Firebase.Auth.AuthError.MissingPassword:
                                errorText.text = ("Please enter your password.");
                                break;
                            case Firebase.Auth.AuthError.InvalidEmail:
                                errorText.text = ("Please enter a valid email address.");
                                break;
                            case Firebase.Auth.AuthError.WeakPassword:
                                errorText.text = ("The password is too weak. Please enter a stronger password.");
                                break;
                            case Firebase.Auth.AuthError.EmailAlreadyInUse:
                                errorText.text = ("An account with this email already exists. Please log in.");
                                break;
                            default:
                                Debug.LogError("Register encountered an error: " + exception.Message);
                                break;
                        }
                        return;
                    }
                    
                    GameManager.instance.currentPlayerID = task.Result.User.UserId;
                });
            }
            StartCoroutine(waitForLogin());

        
        


    }
    /// <summary>
    /// Waits until auth completes, writes username for new users, loads data, and updates UI.
    /// </summary>
    IEnumerator waitForLogin()
    {
        while (GameManager.instance.currentPlayerID == null || GameManager.instance.currentPlayerID == "")
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        if (isLoggingIn == false)
        {
            DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;
            db.Child("players").Child(GameManager.instance.currentPlayerID).Child("username").SetValueAsync(GameObject.Find("UsernameInput").GetComponent<TMP_InputField>().text);
            db.Child("players").Child(GameManager.instance.currentPlayerID).Child("Pets");
            
        }
        
        yield return DatabaseManager.instance.LoadPlayerData(GameManager.instance.currentPlayerID);
        mainMenuUI.SetActive(true);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        yield return new WaitUntil(() => GameManager.instance.currentPlayerName != null && GameManager.instance.currentPlayerName != "");
        Debug.Log("Logged in as: " + GameManager.instance.currentPlayerName);
        mainMenuUI.GetNamedChild("Username").GetComponent<TextMeshProUGUI>().text = GameManager.instance.currentPlayerName;
    }
    /// <summary>
    /// Loads the MainMenu scene.
    /// </summary>
    public void StartButton()
    {
        SoundManager.instance.buttonClick();
    SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitButton()
    {
        SoundManager.instance.buttonClick();
        Application.Quit();
    }
    /// <summary>
    /// Signs out and resets local player state; shows login UI.
    /// </summary>
    public void LogOut()
    {
        SoundManager.instance.buttonClick();
        FirebaseAuth.DefaultInstance.SignOut();
        GameManager.instance.currentPlayerID = null;
        GameManager.instance.currentPlayerPets = new Dictionary<string, Pet>();
        GameManager.instance.currentPlayerName = null;
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }
}
        
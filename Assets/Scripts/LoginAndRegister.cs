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
    private bool isLoggingIn = true;
    public void Awake()
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
    public void changeMenu()
    {
        SoundManager.instance.buttonClick();
        isLoggingIn = !isLoggingIn;
        loginUI.SetActive(isLoggingIn);
        registerUI.SetActive(!isLoggingIn);
    }
    public void LogInOrRegister()
    {
        SoundManager.instance.buttonClick();
        TMP_InputField emailInputField = GameObject.FindGameObjectWithTag("EmailField").GetComponent<TMP_InputField>();
        TMP_InputField passwordInputField = GameObject.FindGameObjectWithTag("PasswordField").GetComponent<TMP_InputField>();
        TextMeshProUGUI errorText = GameObject.Find("ErrorText").GetComponent<TextMeshProUGUI>();
        if (emailInputField.text != "" && passwordInputField.text != "")
        {
            if (isLoggingIn)
            {
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
                    errorText.text = "The Email or Password is incorrect.";
                    return;
                }
                GameManager.instance.currentPlayerID = task.Result.User.UserId;
                
            });
            }
            else
            {
                FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("Register was canceled.");
                        return;
                    }
                    GameManager.instance.currentPlayerID = task.Result.User.UserId;
                });
            }
            StartCoroutine(waitForLogin());

        }
        else
        {
            errorText.text = "Please enter both email and password.";
        }


    }
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
    public void StartButton()
    {
        SoundManager.instance.buttonClick();
    SceneManager.LoadScene("MainMenu");
    }
    public void QuitButton()
    {
        SoundManager.instance.buttonClick();
        Application.Quit();
    }
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
        
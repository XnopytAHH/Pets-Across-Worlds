using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Collections;

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
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }
    public void changeMenu()
    {
        isLoggingIn = !isLoggingIn;
        loginUI.SetActive(isLoggingIn);
        registerUI.SetActive(!isLoggingIn);
    }
    public void LogInOrRegister()
    {
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
    }
    public void StartButton()
    {
    SceneManager.LoadScene("MainMenu");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
        
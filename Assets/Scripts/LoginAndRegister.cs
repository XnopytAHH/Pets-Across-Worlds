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

    private bool isLoggingIn = true;
    public void Awake()
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
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
        SceneManager.LoadScene("MainMenu");
    }
}
        
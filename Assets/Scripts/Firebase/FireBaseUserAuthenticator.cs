using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using TMPro;

public delegate void HasConnectedToDataBase();
public delegate void OnSignIn();
public delegate void OnRegisterNew();
public class FireBaseUserAuthenticator : MonoBehaviour
{
    private static FireBaseUserAuthenticator instance;
    public static FireBaseUserAuthenticator Instance { get { return instance; } }

    public FirebaseAuth auth;
    public TMP_Text email;
    public TMP_Text password;
    public GameObject loginMenu;
    public GameObject mainMenu;

    public static event OnSendMessage onSendMessage;
    public static event HasConnectedToDataBase onDataBaseConnected;
    public static event OnSignIn onSignIn;
    public static event OnRegisterNew onRegisterNew;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals("MainMenu"))
        ConnectToFireBase();
    }

    public void RegisterNewUser()
    {
        Debug.Log("Starting Registration");
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
                onSendMessage?.Invoke("Invalid Email/Password");
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User Registerd: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                SaveManager.Instance.SaveName(email.text);

                onSendMessage?.Invoke("Registration Complete");
                onRegisterNew?.Invoke();

                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        });
    }

    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception.Message);
                onSendMessage?.Invoke("Invalid Email/Password");
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                SaveManager.Instance.SaveName(email.text);
                onSendMessage?.Invoke("Logged In Succuessfully");
                onSignIn?.Invoke();
                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        });
    }
    public void InstantSignIn()
    {
        auth.SignInWithEmailAndPasswordAsync("test2@test.test","Test123!").ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception.Message);
                onSendMessage?.Invoke("Invalid Email/Password");
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                SaveManager.Instance.SaveName("test2@test.test");
                onSendMessage?.Invoke("Logged In Succuessfully");
                onSignIn?.Invoke();
                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        });
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out");
        onSendMessage?.Invoke("Signed Out");
    }
    private void ConnectToFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
                onSendMessage?.Invoke("Failure to connect to Firebase");
            }
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("Connected to Firebase");
            onDataBaseConnected?.Invoke();
        });
    }
}

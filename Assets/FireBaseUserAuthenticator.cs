using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FireBaseUserAuthenticator : MonoBehaviour
{
    private static FireBaseUserAuthenticator instance;
    public static FireBaseUserAuthenticator Instance { get { return instance; } }

    public FirebaseAuth auth;
    public TMP_Text email;
    public TMP_Text password;
    public GameObject loginMenu;
    public GameObject mainMenu;
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
        ConnectToFireBase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterNewUser()
    {
        Debug.Log("Starting Registration");
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User Registerd: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                SaveManager.Instance.SaveName(email.text);
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
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                SaveManager.Instance.SaveName(email.text);
                loginMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        });
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out");
    }
    private void ConnectToFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });
    }
}

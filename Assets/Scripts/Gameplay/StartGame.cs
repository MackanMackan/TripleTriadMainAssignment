using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
public void StartGameOnClick()
    {
        SaveManager.Instance.LoadPlayerDataFromFirebase();
    }
    public void SignIn()
    {
        FireBaseUserAuthenticator.Instance.InstantSignIn();
    }
}

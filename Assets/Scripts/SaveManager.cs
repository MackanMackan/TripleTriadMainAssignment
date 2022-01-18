using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME+name,name);
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME,name);
    }
    public void SaveDeck()
    {
        
    }
}

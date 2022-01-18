using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string PLAYER_NAME = "PLAYER_NAME";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME+name,name);
    }
}

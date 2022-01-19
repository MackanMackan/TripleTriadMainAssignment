using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; }}
    public TMP_Text warningText;

    private static SaveManager instance;
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYER_DECK = "PLAYER_DECK";
    private void Awake()
    {
        if(instance == null)
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
    }


    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME+name,name);
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME,name);
    }
  

    public void SaveDeck(List<GameObject> cardList)
    {
        warningText.text = "Deck Saved!";
        for (int i = 0; i < 5; i++)
        {

        }
    }
}

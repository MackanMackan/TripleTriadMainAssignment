using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; }}
    public TMP_Text warningText;

    PlayerInfoData playerData;
    private static SaveManager instance;
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYERDATA_FILE_ENDING = "InfoData.json";
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
        playerData = new PlayerInfoData();
        playerData.Deck = new List<string>();
    }

    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME+name,name);
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME,name);

        playerData.Name = name;
    }
  

    public void SaveDeck(List<GameObject> cardList)
    {
        warningText.text = "Deck Saved!";
        playerData.Deck.Clear();
        string cardname;
        for (int i = 0; i < 5; i++)
        {
            cardname = cardList[i].GetComponent<CardSettings>().CardName;
            PlayerPrefs.SetString(CURRENT_PLAYER_NAME+"DeckCard" + i, cardname);
            playerData.Deck.Add(cardname);
        }
        SavePlayerDataToFile(playerData);
    }
    public void SavePlayerDataToFile(PlayerInfoData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);

        using (var stream = File.OpenWrite(PlayerPrefs.GetString(CURRENT_PLAYER_NAME) + PLAYERDATA_FILE_ENDING))
        {
            stream.SetLength(0);

            var bytes = Encoding.UTF8.GetBytes(jsonData);

            stream.Write(bytes, 0, bytes.Length);
        }
    }
    public string LoadPlayerDataFromFile(string playerName)
    {
        using (var stream = File.OpenText(PlayerPrefs.GetString(PLAYER_NAME)+ playerName + PLAYERDATA_FILE_ENDING))
        {
            return stream.ReadToEnd();
        }
    }
}

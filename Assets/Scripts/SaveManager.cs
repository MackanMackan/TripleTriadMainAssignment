using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;
using System.Net;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; }}
    public TMP_Text warningText;

    PlayerInfoData playerData;
    private static SaveManager instance;
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYERDATA_FILE_ENDING = "InfoData";
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
    public string GetCurrentPlayerName()
    {
        return PlayerPrefs.GetString(CURRENT_PLAYER_NAME);
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
        SavePlayerData(playerData);
    }
    public void SavePlayerData(PlayerInfoData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);

        SaveToFile(jsonData);
        SaveToJsonSlave(jsonData);
    }
    private void SaveToFile(string jsonData)
    {
        using (var stream = File.OpenWrite(PlayerPrefs.GetString(CURRENT_PLAYER_NAME).ToLower() + PLAYERDATA_FILE_ENDING + ".json"))
        {
            stream.SetLength(0);

            var bytes = Encoding.UTF8.GetBytes(jsonData);

            stream.Write(bytes, 0, bytes.Length);
        }
    }
    private void SaveToJsonSlave(string jsonData)
    {
        var request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/" + PlayerPrefs.GetString(CURRENT_PLAYER_NAME).ToLower() + PLAYERDATA_FILE_ENDING);
        //request.ContentType = "application/json";
        request.Method = "PUT";

        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            streamWriter.Write(jsonData);
            streamWriter.Close();
        }
        var httpResponse = (HttpWebResponse)request.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
            Debug.Log(result);
        }
    }
    public PlayerInfoData LoadPlayerDataFromFile(string playerName)
    {
        StreamReader stream;
        string fileData;
        playerName.ToLower();
        try
        {
            using (stream = File.OpenText(playerName + PLAYERDATA_FILE_ENDING + ".json"))
            {
                fileData = stream.ReadToEnd();
            }

            var request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/" + playerName + PLAYERDATA_FILE_ENDING);
            var response = (HttpWebResponse)request.GetResponse();

            // Open a stream to the server so we can read the response data 
            // it sent back from our GET request
            using (var streamOnline = response.GetResponseStream())
            {
                using (var reader = new StreamReader(streamOnline))
                {
                    string onlineFile = reader.ReadToEnd();
                    Debug.Log(fileData.Equals(onlineFile));
                
                    return JsonUtility.FromJson<PlayerInfoData>(onlineFile);
                }
            }
        }
        catch (Exception)
        {
            Debug.Log("Didn't load correctly, applying Default");
            PlayerInfoData defaultPlayer = new PlayerInfoData();
            defaultPlayer.Name = "Default";
            for (int i = 0; i < 5; i++)
            {
                defaultPlayer.Deck[i] = "Eric Rod";
            }
            return defaultPlayer;
        }
    }
}

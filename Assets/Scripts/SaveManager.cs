using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;
using System.Net;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public delegate void HasLoaded();
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; }}

    public static string PLAYER_TWO => PLAYER_2;

    public static string PLAYER_ONE => PLAYER_1;
    public static event HasLoaded onPlayerLoad;
    public TMP_Text deckBuildingWarningText;
   

    PlayerInfoData playerData;
    GameData gameData;
    private static SaveManager instance;

    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYERDATA_FILE_ENDING = "InfoData";
    private const string PLAYER_2 = "PLAYER2";
    private const string PLAYER_1 = "PLAYER1";
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
        Debug.Log(name.IndexOf("@"));
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME,name.Substring(0, name.IndexOf("@")));

        playerData.Name = name;
    }
    public string GetCurrentPlayerName()
    {
        return PlayerPrefs.GetString(CURRENT_PLAYER_NAME);
    }


    public void SaveDeck(List<GameObject> cardList)
    {
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

        SaveToFirebase(jsonData);
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
    public void LoadPlayerDataFromFirebase()
    {
                
                var db = FirebaseDatabase.DefaultInstance;
                var userId = FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId;
                db.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.Exception != null)
                    {
                        Debug.LogError(task.Exception.Message);
                        playerData = GenerateDefaulPlayerDataInfo();
                        onPlayerLoad?.Invoke();
                    }
                    else
                    {

                        //here we get the result from our database.
                        DataSnapshot snap = task.Result;
                        
                        //And send the json data to a function that can update our game.
                        playerData = ConvertToPlayerInfoData(snap.GetRawJsonValue());
                        onPlayerLoad?.Invoke();
                    }
                });
    }
    public PlayerInfoData GetLoadedPlayer()
    {
        return playerData;
    }
    private PlayerInfoData GenerateDefaulPlayerDataInfo()
    {
        Debug.Log("Didn't load correctly, applying Default");
        PlayerInfoData defaultPlayer = new PlayerInfoData();
        defaultPlayer.Name = "Default";
        defaultPlayer.Deck = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            defaultPlayer.Deck.Add("Eric Rod");
        }
        return defaultPlayer;
    }

    private PlayerInfoData ConvertToPlayerInfoData(string jsonData)
    {

        playerData = JsonUtility.FromJson<PlayerInfoData>(jsonData);
        return playerData;
    }

   
    private void SaveToFirebase(string data)
    {
        Debug.Log("Trying to save to firebase...");
        var db = FirebaseDatabase.DefaultInstance;
        var userId = FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId;
        //puts the json data in the "users/userId" part of the database.
        db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);
                if (deckBuildingWarningText.enabled)
                {
                    deckBuildingWarningText.text = "Something went wrong when trying to save to database: " + task.Exception.Message;
                }
            }
            else
            {
                if (deckBuildingWarningText.enabled)
                {
                    deckBuildingWarningText.text = "Deck Saved!";
                }
            }
        });
    }
}

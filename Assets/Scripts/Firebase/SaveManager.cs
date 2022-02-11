using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Firebase.Database;
using Firebase.Extensions;

public delegate void HasLoaded();
public delegate void OnSendMessage(string warningText);
public delegate void OnGameSessionSaved();
public delegate void OnGameSessionsLoaded();
public delegate void OnStartGameSessionLoaded(GameData data);
public delegate void OnMultiplePlayersLoaded();
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }

    public static string PLAYER_TWO => PLAYER_2;

    public static string PLAYER_ONE => PLAYER_1;

    public List<GameData> GameSessions { get => gameSessions; }
    public PlayerInfoData PlayerData { get => playerData; }

    public static event HasLoaded onPlayerLoad;
    public static event OnSendMessage onSendMessage;
    public static event OnGameSessionSaved onGameSessionSaved;
    public static event OnGameSessionsLoaded onGameSessionsLoaded;
    public static event OnStartGameSessionLoaded onStartGameSessionLoaded;
    public static event OnMultiplePlayersLoaded onMultiplePlayersLoaded;




    PlayerInfoData playerData;
    GameData gameData;
    private static SaveManager instance;
    private List<GameData> gameSessions;
    List<PlayerInfoData> playerDataList;
    FirebaseDatabase db;
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYERDATA_FILE_ENDING = "InfoData";
    private const string PLAYER_2 = "PLAYER2";
    private const string PLAYER_1 = "PLAYER1";
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
        playerData = new PlayerInfoData();
        gameData = new GameData();
        gameSessions = new List<GameData>();
        gameData.playerIDs = new List<string>();
        gameData.cardsOnField = new List<GameObject>();
        playerData.Deck = new List<string>();
        playerData.inGameID = new List<string>();
        playerDataList = new List<PlayerInfoData>();
        FireBaseUserAuthenticator.onDataBaseConnected += SetDatabase;
        FireBaseUserAuthenticator.onSignIn += LoadPlayerDataFromFirebase;
    }

    void SetDatabase()
    {
        db = FirebaseDatabase.DefaultInstance;
    }
    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME + name, name);
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME, name.Substring(0, name.IndexOf("@")));

        playerData.Name = name;
    }
    public string GetCurrentPlayerName()
    {
        return PlayerPrefs.GetString(CURRENT_PLAYER_NAME);
    }

    public void SaveDeck(List<GameObject> cardList)
    {
        PlayerData.Deck.Clear();
        string cardname;
        for (int i = 0; i < 5; i++)
        {
            cardname = cardList[i].GetComponent<CardSettings>().CardName;
            PlayerPrefs.SetString(CURRENT_PLAYER_NAME + "DeckCard" + i, cardname);
            PlayerData.Deck.Add(cardname);
        }
        SaveUserToFirebase();
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
    public void LoadPlayerDataFromFirebase()
    {
        var userId = FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId;
        LoadPlayerDataFromFirebase(userId);
    }
    public void LoadPlayerDataFromFirebase(string userId)
    {
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
                Debug.Log("Loading was success");
                //And send the json data to a function that can update our game.
                playerData = ConvertToPlayerInfoData(snap.GetRawJsonValue());
                onPlayerLoad?.Invoke();
            }
        });
    }
    public void LoadMultiplePlayerDataFromFirebase(List<string> userIds)
    {
        playerDataList.Clear();
        for (int i = 0; i < 2; i++)
        {
            db.RootReference.Child("users").Child(userIds[i]).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception.Message);
                    playerData = GenerateDefaulPlayerDataInfo();
                }
                else
                {

                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;
                    Debug.Log("Loading multiple players was success");
                    //And send the json data to a function that can update our game.
                    playerData = ConvertToPlayerInfoData(snap.GetRawJsonValue());
                    playerDataList.Add(playerData);
                    if (i == 1)
                    {
                        onMultiplePlayersLoaded?.Invoke();
                    }
                }
            });
        }
    }
    public List<PlayerInfoData> GetPlayersDataForGameSession()
    {
        return playerDataList;
    }
    public PlayerInfoData GetLoadedPlayer()
    {
        return PlayerData;
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
        return PlayerData;
    }


    public void SaveUserToFirebase()
    {
        Debug.Log("Trying to save to firebase...");
        var userId = FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId;
        string jsonData = JsonUtility.ToJson(playerData);
        //puts the json data in the "users/userId" part of the database.
        var dataRef = db.RootReference.Child("users").Child(userId);
        //SaveToDataFireBase(dataRef,data);
        db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);

                onSendMessage?.Invoke("Something went wrong when trying to save to database: " + task.Exception.Message);
            }
            else
            {
                onSendMessage?.Invoke("Deck Saved!");
            }
        });
    }
    public void CreateNewGameSession()
    {
        string key = db.RootReference.Child("games/").Push().Key;
        playerData.inGameID.Add(key);
        gameData.gameID = key;
        gameData.players++;
        gameData.playerIDs.Add(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId);
        gameData.displayName = PlayerPrefs.GetString(CURRENT_PLAYER_NAME) + "´s Game";
        for (int i = 0; i < 9; i++)
        {
            gameData.cardsOnField.Add(null);
        }
        SaveGameSession(gameData, key);
    }
    public void LoadGameSessions()
    {
        db.RootReference.Child("games/").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);
            }
            else
            {

                DataSnapshot snap = task.Result;
                Debug.Log("Loading Sessions was successful");
                GameSessions.Clear();
                foreach (var child in snap.Children)
                {
                    GameData data = JsonUtility.FromJson<GameData>(child.GetRawJsonValue());
                    GameSessions.Add(data);
                }
                onGameSessionsLoaded?.Invoke();
            }
        });
    }
    public void LoadPlayerGameSession(string gameId)
    {
        db.RootReference.Child("games/").Child(gameId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);
            }
            else
            {
                DataSnapshot snap = task.Result;
                Debug.Log("Loading Game Session was successful: "+gameId);
                GameData data = JsonUtility.FromJson<GameData>(snap.GetRawJsonValue());
                onStartGameSessionLoaded?.Invoke(data);
            }
        });
    }
    public void SaveGameSession(GameData data, string key)
    {
        string path = "games/" + key;
        string json = JsonUtility.ToJson(data);
        db.RootReference.Child(path).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);

                onSendMessage?.Invoke("Something went wrong when trying to save to database: " + task.Exception.Message);
            }
            else
            {
                Debug.Log("Saved:" + data.gameID);
                onGameSessionSaved?.Invoke();
            }
        });
    }
    void SaveToDataFireBase(DatabaseReference reference, object data)
    {
        string jsonData = JsonUtility.ToJson(data);
        reference.SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception.Message);

                onSendMessage?.Invoke("Something went wrong when trying to save to database: " + task.Exception.Message);
            }
            else
            {
                onGameSessionSaved?.Invoke();
            }
        });
    }
}

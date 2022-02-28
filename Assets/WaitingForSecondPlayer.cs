using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
public class WaitingForSecondPlayer : MonoBehaviour
{
    public GameObject waitngForPlayer;
    ActiveMatchGameDataHandler matchDataHandler;
    GetPlayerDecks getPLDecks;
    MatchManager matchManager;
    void Start()
    {
        matchDataHandler = transform.parent.GetComponent<ActiveMatchGameDataHandler>();
        getPLDecks = transform.parent.GetComponent<GetPlayerDecks>();
        matchManager = transform.parent.GetComponent<MatchManager>();
        SetDataBaseListener();
    }

    void StartGame(object sender, ValueChangedEventArgs args)
    {
        string players = args.Snapshot.Value.ToString();
        if (players.Equals("2")){
            waitngForPlayer.SetActive(false);
            getPLDecks.enabled = true;
            matchDataHandler.enabled = true;
            matchManager.enabled = true;
        }
    }
    void SetDataBaseListener()
    {
        Debug.Log(SaveManager.Instance.PlayerData.inGameID);
        FirebaseDatabase.DefaultInstance.RootReference.Child("games").Child(SaveManager.Instance.PlayerData.inGameID).Child("players").ValueChanged += StartGame;
    }
}

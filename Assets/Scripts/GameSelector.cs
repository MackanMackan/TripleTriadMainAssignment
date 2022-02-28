using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSelector : MonoBehaviour
{
    public static event OnSendMessage onSendMessage;

    bool alreadyInGame = false;
    private void Start()
    {
        SaveManager.onGameSessionsLoaded += JoinGameSession;
        SaveManager.onGameSessionSaved += ChangeSceneOnJoin;
        alreadyInGame = false;
    }
    public void LoadGameSessions()
    {
        onSendMessage?.Invoke("Looking For Games...");
        SaveManager.Instance.LoadGameSessions();
    }
    GameData CheckIfEmptySpotInGame()
    {
        foreach (var game in SaveManager.Instance.GameSessions)
        {
            Debug.Log("Checking");
            string userId = FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId;
                if (userId.Equals(game.playerIDs[0]) 
                || userId.Equals(game.playerIDs[1]))
                {
                    alreadyInGame = true;
                    return game;
                }

        }
        foreach (var game in SaveManager.Instance.GameSessions)
        {
            if (game.players < 2)
            {
                return game;
            }
        }
        return null;
    }
    void JoinGameSession()
    {
        GameData data = CheckIfEmptySpotInGame();
        if (data != null && !alreadyInGame)
        {
            SaveManager.Instance.SetPlayerGameID(data.gameID);
            data.players++;
            data.playerIDs.Add(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId);
            SaveManager.Instance.SaveGameSession(data, data.gameID);
            Debug.Log("Joining");
        }
        else if(!alreadyInGame)
        {
            SaveManager.Instance.CreateNewGameSession();
            Debug.Log("Creating");
        }
        else
        {
            ChangeSceneOnJoin();
            SaveManager.Instance.SetPlayerGameID(data.gameID);
            Debug.Log("Joining active game");
        }
        SaveManager.Instance.SaveUserToFirebase();
    }
    void ChangeSceneOnJoin()
    {
        SceneController.Instance.LoadScene("Game");
        SaveManager.onGameSessionSaved -= ChangeSceneOnJoin;
    }
}

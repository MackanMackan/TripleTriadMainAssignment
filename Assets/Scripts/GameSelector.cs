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
            foreach (var playerGameId in SaveManager.Instance.PlayerData.inGameID)
            {
                if (playerGameId.Equals(game.gameID))
                {
                    alreadyInGame = true;
                    return game;
                }

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
            SaveManager.Instance.PlayerData.inGameID.Add(data.gameID);
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
            Debug.Log("Joining active game");
        }
        SaveManager.Instance.SaveUserToFirebase();
        alreadyInGame = false;
    }
    void ChangeSceneOnJoin()
    {
        SceneController.Instance.LoadScene("Game");
    }
}

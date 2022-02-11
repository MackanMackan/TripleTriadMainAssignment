using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSelector : MonoBehaviour
{
    public static event OnSendMessage onSendMessage;

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
        if(data != null)
        {
            SaveManager.Instance.PlayerData.inGameID.Add(data.gameID);
            data.players++;
            data.playerIDs.Add(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId);
            SaveManager.Instance.SaveGameSession(data,data.gameID);
            Debug.Log("Joining");
        }
        else
        {
            SaveManager.Instance.CreateNewGameSession();
            Debug.Log("Creating");
        }
    }
    void ChangeSceneOnJoin()
    {
        SceneController.Instance.LoadScene("Game");
    }
}

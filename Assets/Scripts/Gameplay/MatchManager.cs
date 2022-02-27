using System.Collections;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void OnGameOver();
public class MatchManager : MonoBehaviour
{
    public List<GameObject> cardPlaceHolders;
    [SerializeField]
    TMP_Text playerWinnerText;
    int turns;
    GameObject gameBoard;

    static int player1Score = 0;
    static int player2Score = 0;

    public static event OnGameOver onGameOver;
    void Start()
    {
        SaveManager.onStartGameSessionLoaded += SetThisMatchDatabaseListener;
        gameBoard = GameObject.Find("GameBoard");
        //TODO: flip coin who starts, and add one score to the other player to balance scoreing out
    }
    void SetThisMatchDatabaseListener(GameData gameData)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games").Child(gameData.gameID).Child("numberOfTurns").ValueChanged += CheckIfGameIsOver;
        SaveManager.onStartGameSessionLoaded -= SetThisMatchDatabaseListener;
    }
    void CheckIfGameIsOver(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        int turns = int.Parse(args.Snapshot.Value.ToString());
        if (turns == 9)
        {
            CalculateWinner();
        }
    }
    void CalculateWinner()
    {
        string winner;

        ScanGameBoardFrames();
        if (player1Score == player2Score)
        {
            winner = "Stalemate";
        }
        else
        {
            winner = player1Score > player2Score ? PlayerPrefs.GetString("PLAYER1") : PlayerPrefs.GetString("PLAYER2");
        }

        RemoveBoard();
        ShowWinner(winner);
    }
    void RemoveBoard()
    {
        gameBoard.SetActive(false);
    }
    void ShowWinner(string winner)
    {
        playerWinnerText.transform.parent.gameObject.SetActive(true);
        playerWinnerText.text = winner.Equals("Stalemate") ? winner+"!" : winner + " Wins!";
        onGameOver?.Invoke();
    }
    public static void ReportPlayerScore(Sprite frame)
    {
        if(frame.name.Equals("RedFrame"))
        {
            player1Score++;
            player2Score--;
        }
        else
        {
            player1Score--;
            player2Score++;
        }
    }
    public static void AddScoreWhenPlayingCard(Sprite frame)
    {
        if (frame.name.Equals("RedFrame"))
        {
            player1Score++;
        }
        else
        {
            player2Score++;
        }
    }
    void SaveGameData(GameData gameData)
    {
        SaveManager.Instance.SaveGameSession(gameData, gameData.gameID);
    }
    void ScanGameBoardFrames()
    {
        string frame;
        for (int i = 0; i < 9; i++)
        {
            //checks all the frames and sees whos the winner based on frames on board.
            frame = cardPlaceHolders[i].transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite.name;
            if (frame.Equals("RedFrame"))
            {
                player1Score++;
            }
            else
            {
                player2Score++;
            }
        }
    }
}

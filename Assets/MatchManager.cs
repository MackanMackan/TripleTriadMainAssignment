using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchManager : MonoBehaviour, IChangeTurnObserver
{
    [SerializeField]
    TMP_Text playerWinnerText;
    int turns;
    ChangePlayerTurn changePlayerTurnObserver;
    GameObject gameBoard;

    static int player1Score = 0;
    static int player2Score = 0;

    void Start()
    {
        changePlayerTurnObserver = GameObject.Find("Canvas").GetComponent<ChangePlayerTurn>();
        changePlayerTurnObserver.AddChangeTurnObserver(gameObject.GetComponent<MatchManager>());
        gameBoard = GameObject.Find("GameBoard");
        //TODO: flip coin who starts, and add one score to the other player to balance scoreing out
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NotifyChangeTurn(bool playerOneTurn)
    {
        turns++;
        if(turns == 9)
        {
            if (playerOneTurn)
            {
                player1Score++;
            }
            else
            {
                player2Score++;
            }

            CalculateWinner();
        }
    }
    void CalculateWinner()
    {
        string winner;

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
}

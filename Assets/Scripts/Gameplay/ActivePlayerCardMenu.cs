using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlayerCardMenu : MonoBehaviour
{
    public GameObject cardListButton;
    GameObject player1CardMenu;
    GameObject player2CardMenu;
    void Start()
    {
        player1CardMenu = GameObject.Find("PlayerOneCardMenu");
        player2CardMenu = GameObject.Find("PlayerTwoCardMenu");
        SaveManager.onGameSessionSaved += ChangeActiveCardMenu;
    }

    public void ChangeActiveCardMenu()
    {
        string playerGameId = ActiveMatchGameDataHandler.activeGameData.playerIDs[0];
        bool player1Turn = ActiveMatchGameDataHandler.activeGameData.playerTurn;
        if (player1Turn && playerGameId.Equals(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId))
        {
            player1CardMenu.SetActive(true);
            cardListButton.SetActive(true);
        }
        else
        {
            player1CardMenu.SetActive(false);
            cardListButton.SetActive(false);
        }

        playerGameId = ActiveMatchGameDataHandler.activeGameData.playerIDs[1];

        if (!player1Turn && playerGameId.Equals(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId))
        {
            player2CardMenu.SetActive(true);
            cardListButton.SetActive(true);
        }
        else
        {
            player2CardMenu.SetActive(false);
            cardListButton.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveCardList : MonoBehaviour
{
    GameObject player1CardList;
    GameObject player2CardList;

    void Start()
    {
        ChangePlayerTurn.onChangeTurn += NotifyChangeTurn;
        player1CardList = GameObject.Find("PlayerOneCardMenu");
        player2CardList = GameObject.Find("PlayerTwoCardMenu");
    }

    public void NotifyChangeTurn(bool playerOneTurn)
    {
        player1CardList.SetActive(playerOneTurn);
        player2CardList.SetActive(!playerOneTurn);
    }
}

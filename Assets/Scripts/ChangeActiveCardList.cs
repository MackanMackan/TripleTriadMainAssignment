using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveCardList : MonoBehaviour, IChangeTurnObserver
{
    ChangePlayerTurn changePlayerTurnObserver;
    GameObject player1CardList;
    GameObject player2CardList;

    void Start()
    {
        changePlayerTurnObserver = GameObject.Find("Canvas").GetComponent<ChangePlayerTurn>();
        changePlayerTurnObserver.AddChangeTurnObserver(gameObject.GetComponent<ChangeActiveCardList>());

        player1CardList = GameObject.Find("PlayerOneCardMenu");
        player2CardList = GameObject.Find("PlayerTwoCardMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NotifyChangeTurn(bool playerOneTurn)
    {
        player1CardList.SetActive(playerOneTurn);
        player2CardList.SetActive(!playerOneTurn);
    }
}

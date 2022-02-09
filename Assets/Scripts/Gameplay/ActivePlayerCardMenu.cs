using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivePlayerCardMenu : MonoBehaviour, IChangeTurnObserver
{

    GameObject player1CardMenu;
    GameObject player2CardMenu;
    void Start()
    {
        player1CardMenu = GameObject.Find("PlayerOneCardMenu");
        player2CardMenu = GameObject.Find("PlayerTwoCardMenu");
    }

    public void NotifyChangeTurn(bool playerOneTurn)
    {
        if (player1CardMenu)
        {
            player1CardMenu.SetActive(true);
            player2CardMenu.SetActive(false);
        }
        else
        {
            player1CardMenu.SetActive(false);
            player2CardMenu.SetActive(true);
        }
    }
}

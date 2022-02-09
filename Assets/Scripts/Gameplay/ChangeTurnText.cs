using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTurnText : MonoBehaviour, IChangeTurnObserver
{
    TMP_Text turnText;
    ChangePlayerTurn changePlayerTurnObserver;


    void Start()
    {
        turnText = GetComponent<TMP_Text>();
        changePlayerTurnObserver = GameObject.Find("Canvas").GetComponent<ChangePlayerTurn>();
        changePlayerTurnObserver.AddChangeTurnObserver(gameObject.GetComponent<ChangeTurnText>());
        turnText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NotifyChangeTurn(bool playerOneTurn)
    {
        int player = playerOneTurn ? 1 : 2;
        turnText.text = PlayerPrefs.GetString("PLAYER"+player) + " turn!";
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTurnText : MonoBehaviour
{
    TMP_Text turnText;


    void Start()
    {
        turnText = GetComponent<TMP_Text>();
        ChangePlayerTurn.onChangeTurn += NotifyChangeTurn;
        turnText.text = "";
    }

    public void NotifyChangeTurn(bool playerOneTurn)
    {
        int player = playerOneTurn ? 1 : 2;
        turnText.text = PlayerPrefs.GetString("PLAYER"+player) + " turn!";
    }
}

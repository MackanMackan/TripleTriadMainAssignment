using System;
using UnityEngine;
[Serializable]
public class GameData
{
    public string player1UserId;
    public string player2UserId;
    public int player1Score;
    public int player2Score;
    public GameObject[] cardsOnField;
}
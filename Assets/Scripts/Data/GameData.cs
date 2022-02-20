using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GameData
{
    public string displayName;
    public List<string> playerIDs;
    public string[] cardsOnField;
    public string gameID;
    public int players;
    public bool playerTurn = true;
    public int numberOfTurns = 0;
}
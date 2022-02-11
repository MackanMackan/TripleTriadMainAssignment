using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GameData
{
    public string displayName;
    public List<string> playerIDs;
    public List<GameObject> cardsOnField;
    public string gameID;
    public int players;
    public bool playerTurn = true;
}
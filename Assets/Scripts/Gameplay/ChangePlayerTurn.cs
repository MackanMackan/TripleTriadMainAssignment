using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnChangeTurn(bool playerTurn);
public class ChangePlayerTurn : MonoBehaviour
{
    public static event OnChangeTurn onChangeTurn;

    bool playerOneTurn = true;
    public void ChangePlayersTurn()
    {
        playerOneTurn = !SaveManager.Instance.gameData.playerTurn;
        onChangeTurn?.Invoke(playerOneTurn);
    }
}

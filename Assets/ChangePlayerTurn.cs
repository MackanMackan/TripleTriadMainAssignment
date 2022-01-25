using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerTurn : MonoBehaviour
{
    private List<IChangeTurnObserver> observers = new List<IChangeTurnObserver>();
    bool playerOneTurn = true;

    public void AddChangeTurnObserver(IChangeTurnObserver observer)
    {
        observers.Add(observer);
    }
    public void NotifyChangingPlayerTurn(bool playerOneTurn)
    {
        foreach (var observer in observers)
        {
            observer.NotifyChangeTurn(playerOneTurn);
        }
    }
    public void ChangeTurn()
    {
        playerOneTurn = !playerOneTurn;
        NotifyChangingPlayerTurn(playerOneTurn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangeTurnObserver
{
    void NotifyChangeTurn(bool playerOneTurn);
}

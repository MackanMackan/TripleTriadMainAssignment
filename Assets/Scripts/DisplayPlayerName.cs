using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPlayerName : MonoBehaviour
{
    public TMP_Text playerName;
    void Start()
    {
        playerName.text = "Welcome " + SaveManager.Instance.GetCurrentPlayerName()+"!";
    }

}

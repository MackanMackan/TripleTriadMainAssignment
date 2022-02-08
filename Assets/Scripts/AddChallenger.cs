using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddChallenger : MonoBehaviour
{
    [SerializeField]
    TMP_Text warningText;

    [SerializeField]
    GameObject challengeBlocker;

    TMP_InputField challenger;

    public static event OnSendMessage onSendMessage;
    public void GetChallenger(TMP_InputField challenger)
    {
        onSendMessage?.Invoke("Searching for "+ challenger.text);
        this.challenger = challenger;
        Invoke(nameof(SearchForPayer),0.2f);
       
    }
    void SearchForPayer()
    {
        if (SaveManager.Instance.GetLoadedPlayer().Name.Equals("Default"))
        {
            onSendMessage?.Invoke("Could not find " + challenger.text + "!");
            challengeBlocker.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString(SaveManager.PLAYER_TWO, challenger.text);
            onSendMessage?.Invoke(challenger.text + " found!");
            challengeBlocker.SetActive(true);
        }
    }
}

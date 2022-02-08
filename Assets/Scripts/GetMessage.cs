using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetMessage : MonoBehaviour
{
    public TMP_Text messageText;
    private void OnEnable()
    {
        SaveManager.onSendMessage += SendWarningMessage;
        FindPlayerDeck.onSendMessage += SendWarningMessage;
        AddChallenger.onSendMessage += SendWarningMessage;
        FireBaseUserAuthenticator.onSendMessage += SendWarningMessage;
    }
    private void OnDisable()
    {
        SaveManager.onSendMessage -= SendWarningMessage;
        FindPlayerDeck.onSendMessage -= SendWarningMessage;
        AddChallenger.onSendMessage -= SendWarningMessage;
        FireBaseUserAuthenticator.onSendMessage -= SendWarningMessage;
    }
    private void OnDestroy()
    {
        SaveManager.onSendMessage -= SendWarningMessage;
        FindPlayerDeck.onSendMessage -= SendWarningMessage;
        AddChallenger.onSendMessage -= SendWarningMessage;
        FireBaseUserAuthenticator.onSendMessage -= SendWarningMessage;
    }

    void SendWarningMessage(string message)
    {
        messageText.text = message;
    }
}

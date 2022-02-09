using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckConnector : MonoBehaviour
{
    private TMP_Text deckText;
    public void GetDecktext(TMP_Text text)
    {
        deckText = text;
    }
    public void ApplyCardName(string cardName)
    {
        deckText.text = cardName;
    }
}

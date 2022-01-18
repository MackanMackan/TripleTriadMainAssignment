using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckConnector : MonoBehaviour
{
    public TMP_Text deckText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetDecktext(TMP_Text text)
    {
        deckText = text;
    }
    public void ApplyCardName(string cardName)
    {
        deckText.text = cardName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckValidDeck : MonoBehaviour
{
    public TMP_Text warningText;

    private List<GameObject> cardValidationList;
    CardPrefabGetter cardGetter;

    public static event OnSendMessage onSendMessage;
    void Start()
    {
        cardValidationList = new List<GameObject>();
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
    }
    public void ValidateDeck()
    {
        cardValidationList.Clear();
        TMP_Text card1 = GameObject.Find("Card0").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card2 = GameObject.Find("Card1").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card3 = GameObject.Find("Card2").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card4 = GameObject.Find("Card3").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card5 = GameObject.Find("Card4").transform.GetChild(0).GetComponent<TMP_Text>();

        

        cardValidationList.Add(cardGetter.GetCard(card1.text));
        cardValidationList.Add(cardGetter.GetCard(card2.text));
        cardValidationList.Add(cardGetter.GetCard(card3.text));
        cardValidationList.Add(cardGetter.GetCard(card4.text));
        cardValidationList.Add(cardGetter.GetCard(card5.text));

        
        //Check if you have two of the same card
        foreach (GameObject card in cardValidationList)
        {
            if(card == null)
            {
                onSendMessage?.Invoke("You most fill all card slots to save!");
                return;
            }
            int doubles = 0;
            for (int i = 0; i < 5; i++)
            {
                
                if (card.GetComponent<CardSettings>().CardName.Equals(cardValidationList[i].GetComponent<CardSettings>().CardName))
                {
                    doubles++;
                    if (doubles == 2)
                    {
                        onSendMessage?.Invoke("You are not allowed to have more than one of each card!");
                        return;
                    }
                }
            }
        }
        if (!ValidateStars(cardValidationList))
        {
            onSendMessage?.Invoke("You may only have two 4 stars cards or one 4 star and one 5 star card in the deck!");
            return;
        }
        SaveManager.Instance.SaveDeck(cardValidationList);
    }
    bool ValidateStars(List<GameObject> cards)
    {
        int amountOf4Stars = 2;
        int amountOf5Stars = 1;
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<CardSettings>().StarLevel == 4)
            {
                amountOf4Stars--;
            }
            else if (card.GetComponent<CardSettings>().StarLevel == 5)
            {
                amountOf4Stars--;
                amountOf5Stars--;
            }
            if (amountOf4Stars < 0 || amountOf5Stars < 0)
            {
                return false;
            }
        }
        return true;
    }
}

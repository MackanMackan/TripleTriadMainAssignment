using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveManager : MonoBehaviour
{
    private const string PLAYER_NAME = "PLAYER_NAME";
    private const string CURRENT_PLAYER_NAME = "CURRENT_PLAYER_NAME";
    private const string PLAYER_DECK = "PLAYER_DECK";
    public TMP_Text warningText;
    private List<GameObject> cardList;
    private List<GameObject> cardValidationList;
    CardPrefabGetter cardGetter;
    void Start()
    {
        cardGetter = GetComponent<CardPrefabGetter>();
        DontDestroyOnLoad(gameObject);
        cardList = new List<GameObject>();
        cardValidationList = new List<GameObject>();
    }


    public void SaveName(string name)
    {
        PlayerPrefs.SetString(PLAYER_NAME+name,name);
        PlayerPrefs.SetString(CURRENT_PLAYER_NAME,name);
    }
    public void CheckValidDeck()
    {
        cardValidationList.Clear();
        TMP_Text card1 = GameObject.Find("Card0").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card2 = GameObject.Find("Card1").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card3 = GameObject.Find("Card2").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card4 = GameObject.Find("Card3").transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text card5 = GameObject.Find("Card4").transform.GetChild(0).GetComponent<TMP_Text>();

        int amountOf4Stars = 2;
        int amountOf5Stars = 1;

        cardValidationList.Add(cardGetter.GetCard(card1.text));
        cardValidationList.Add(cardGetter.GetCard(card2.text));
        cardValidationList.Add(cardGetter.GetCard(card3.text));
        cardValidationList.Add(cardGetter.GetCard(card4.text));
        cardValidationList.Add(cardGetter.GetCard(card5.text));


        //Check if doubles
        int doubles = 0;
        foreach (GameObject card in cardValidationList)
        {
            for (int i = 0; i < 5; i++)
            {
                if (card.GetComponent<CardSettings>().CardName.Equals(cardValidationList[i].GetComponent<CardSettings>().CardName))
                {
                    doubles++;
                    if(doubles == 2)
                    {
                        warningText.text = "You are not allowed to have more than one of each card!";
                        return;
                    }
                }
            }
        }

        SaveDeck();
    }

    void SaveDeck()
    {
        for (int i = 0; i < 5; i++)
        {

        }
    }
}

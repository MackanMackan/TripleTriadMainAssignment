using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using TMPro;
public class FindPlayerDeck : MonoBehaviour
{
    TMP_InputField playerName;
    PlayerInfoData playerInfo;
    TMP_Text findWarningText;
    GameObject[] cardPlaceholders;
    CardPrefabGetter cardGetter;
    List<GameObject> tempList;
    void Start()
    {
        playerName = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
        findWarningText = GameObject.Find("FindWarningText").GetComponent<TMP_Text>();
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        cardPlaceholders = new GameObject[5];
        playerInfo = new PlayerInfoData();
        tempList = new List<GameObject>();
        for (int i = 0; i < cardPlaceholders.Length; i++)
        {
            cardPlaceholders[i] = GameObject.Find("CardPlaceholder" + i);
        }

    }
    public void AddPlayerDeckToList()
    {
        foreach (var card in tempList)
        {
            Destroy(card);
        }

        tempList.Clear();

        playerInfo = SaveManager.Instance.LoadPlayerDataFromJsonSlave(playerName.text);
        if (playerInfo.Name.Equals("Default"))
        {
            findWarningText.text = "Couldn't find player";
        }
        else
        {
            DisplayListOnScreen();
        }
    }
    private void DisplayListOnScreen()
    {
        for (int i = 0; i < 5; i++)
        {
            var instanceCard = Instantiate(cardGetter.GetCard(playerInfo.Deck[i]), cardPlaceholders[i].transform);
            instanceCard.transform.position = cardPlaceholders[i].transform.position;
            tempList.Add(instanceCard);
        }
    }
}

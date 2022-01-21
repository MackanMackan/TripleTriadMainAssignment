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
    void Start()
    {
        playerName = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
        findWarningText = GameObject.Find("FindWarningText").GetComponent<TMP_Text>();
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        cardPlaceholders = new GameObject[5];
        playerInfo = new PlayerInfoData();
        for (int i = 0; i < cardPlaceholders.Length; i++)
        {
            cardPlaceholders[i] = GameObject.Find("CardPlaceholder" + i);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPlayerDeckToList()
    {
        //TODO: Clear list of instatiated cards!
        try
        {
            string jsonData = SaveManager.Instance.LoadPlayerDataFromFile(playerName.text);
            playerInfo = JsonUtility.FromJson<PlayerInfoData>(jsonData);
        }
        catch (Exception)
        {
            findWarningText.text = "Could not find player!";
            return;
        }
        DisplayListOnScreen();
    }
    private void DisplayListOnScreen()
    {
        for (int i = 0; i < 5; i++)
        {
            var instanceCard = Instantiate(cardGetter.GetCard(playerInfo.Deck[i]),cardPlaceholders[i].transform);
            instanceCard.transform.position = Vector3.zero;
        }
    }
}

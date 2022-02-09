using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using TMPro;

public class FindPlayerDeck : MonoBehaviour
{
    PlayerInfoData playerInfo;
    GameObject[] cardPlaceholders;
    CardPrefabGetter cardGetter;
    List<GameObject> tempList;

    public static event OnSendMessage onSendMessage;
    private void OnEnable()
    {
        SaveManager.onPlayerLoad += GetLoadedPlayer;
    }
    private void OnDisable()
    {
        SaveManager.onPlayerLoad -= GetLoadedPlayer;
    }
    private void OnDestroy()
    {
        SaveManager.onPlayerLoad -= GetLoadedPlayer;
    }
    void Start()
    {
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
        onSendMessage?.Invoke("Searching For Player...");
        SaveManager.Instance.LoadPlayerDataFromFirebase();
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
    void LoadPlayerToList()
    {
        
        if (playerInfo.Name.Equals("Default"))
        {
            onSendMessage?.Invoke("Couldn't find player");
        }
        else
        {
            DisplayListOnScreen();
            onSendMessage?.Invoke("");
        }
    }
    void GetLoadedPlayer(PlayerInfoData playerData)
    {
        playerInfo = playerData;
        LoadPlayerToList();
    }
}

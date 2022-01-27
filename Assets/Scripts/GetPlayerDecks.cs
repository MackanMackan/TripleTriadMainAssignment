using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerDecks : MonoBehaviour
{
    public Sprite playerTwoCardFrame;
    PlayerInfoData player1;
    PlayerInfoData player2;
    GameObject cardMenuPlOne;
    GameObject cardMenuPlTwo;
    GameObject cardMenus;
    CardPrefabGetter cardGetter;
    PlaceCardInField placeCardInField;
    void Start()
    {
        cardMenuPlOne = GameObject.Find("PlayerOneCardMenu");
        cardMenuPlTwo = GameObject.Find("PlayerTwoCardMenu");
        cardMenus = GameObject.Find("CardMenus");
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        placeCardInField = GameObject.Find("Canvas").GetComponent<PlaceCardInField>();

        AddPlayerOneDeck();
        AddPlayerTwoDeck();
    }


    private void AddPlayerOneDeck()
    {
        try
        {
            player1 = SaveManager.Instance.LoadPlayerDataFromJsonSlave(SaveManager.Instance.GetCurrentPlayerName());
            PlayerPrefs.SetString("PLAYER1",player1.Name);
        }
        catch (Exception)
        {
            Debug.Log("Could not find player! Choosing Default");
            player1 = SaveManager.Instance.LoadPlayerDataFromJsonSlave("Default");
            return;
        }
        AddPlayerOneDeckToCardMenu();
    }
    private void AddPlayerOneDeckToCardMenu()
    {
        for (int i = 0; i < 5; i++)
        {
            var instancedCard = Instantiate(cardGetter.GetCard(player1.Deck[i]), cardMenuPlOne.transform);
            instancedCard.transform.localPosition = new Vector2(-420,(Camera.main.pixelHeight/5)*i - 1222);
            Button instanceButton = instancedCard.GetComponent<Button>();
            instanceButton.onClick.AddListener(cardMenus.GetComponent<MoveChooseCardMenu>().MoveMenuAwayFromScreen);
            instanceButton.onClick.AddListener(delegate { placeCardInField.ChooseCardToPlay(instancedCard); });
        }
    }
    private void AddPlayerTwoDeck()
    {
        try
        {
            player2 = SaveManager.Instance.LoadPlayerDataFromJsonSlave("Mackelashni");
            PlayerPrefs.SetString("PLAYER2", player2.Name);
        }
        catch (Exception)
        {
            Debug.Log("Could not find player! Choosing Default");
            player2 = SaveManager.Instance.LoadPlayerDataFromJsonSlave("Default");
            return;
        }
        AddPlayerTwoDeckToCardMenu();
    }
    private void AddPlayerTwoDeckToCardMenu()
    {
        for (int i = 0; i < 5; i++)
        {
            var instancedCard = Instantiate(cardGetter.GetCard(player2.Deck[i]), cardMenuPlTwo.transform);
            instancedCard.transform.localPosition = new Vector2(-420, (Camera.main.pixelHeight / 5) * i - 1222);
            instancedCard.transform.GetChild(2).GetComponent<Image>().sprite = playerTwoCardFrame;
            Button instanceButton = instancedCard.GetComponent<Button>();
            instanceButton.onClick.AddListener(cardMenus.GetComponent<MoveChooseCardMenu>().MoveMenuAwayFromScreen);
            instanceButton.onClick.AddListener(delegate { placeCardInField.ChooseCardToPlay(instancedCard); });
        }
        cardMenuPlTwo.SetActive(false);
    }
}

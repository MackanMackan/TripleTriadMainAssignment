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
    private void OnEnable()
    {
        SaveManager.onPlayerLoad += AddPlayerOneDeck;
        SaveManager.onPlayerLoad += AddPlayerTwoDeck;
    }
    private void OnDisable()
    {
        SaveManager.onPlayerLoad -= AddPlayerOneDeck;
        SaveManager.onPlayerLoad -= AddPlayerTwoDeck;
    }
    private void OnDestroy()
    {
        SaveManager.onPlayerLoad -= AddPlayerOneDeck;
        SaveManager.onPlayerLoad -= AddPlayerTwoDeck;
    }
    void Start()
    {
        cardMenuPlOne = GameObject.Find("PlayerOneCardMenu");
        cardMenuPlTwo = GameObject.Find("PlayerTwoCardMenu");
        cardMenus = GameObject.Find("CardMenus");
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        placeCardInField = GameObject.Find("Canvas").GetComponent<PlaceCardInField>();

        SaveManager.Instance.LoadPlayerDataFromFirebase();
    }


    private void AddPlayerOneDeck()
    {
        try
        {
            player1 = SaveManager.Instance.GetLoadedPlayer();
            PlayerPrefs.SetString(SaveManager.PLAYER_ONE,player1.Name);
        }
        catch (Exception)
        {
            Debug.Log("Could not find player! Choosing Default");
            player1 = SaveManager.Instance.GetLoadedPlayer();
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
        //TODO LOAD PLAYER 2
        try
        {
            player2 = SaveManager.Instance.GetLoadedPlayer();
        }
        catch (Exception)
        {
            Debug.Log("Could not find player! Choosing Default");
            player2 = SaveManager.Instance.GetLoadedPlayer();
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

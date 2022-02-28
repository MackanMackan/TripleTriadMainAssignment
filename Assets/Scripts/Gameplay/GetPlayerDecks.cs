using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Collections;

public class GetPlayerDecks : MonoBehaviour
{
    public Sprite playerTwoCardFrame;
    List<PlayerInfoData> playDataList;
    PlayerInfoData player1;
    PlayerInfoData player2;
    GameObject cardMenuPlOne;
    GameObject cardMenuPlTwo;
    GameObject cardMenus;
    CardPrefabGetter cardGetter;
    PlaceCardInField placeCardInField;
    private void OnEnable()
    {
        SaveManager.onPlayerLoad += LoadGameSessionFromPlayer;
        SaveManager.onStartGameSessionLoaded += LoadPlayers;
        SaveManager.onMultiplePlayersLoaded += CallCorutineForGettingLoadedPlayers;
    }
    private void OnDisable()
    {
        SaveManager.onPlayerLoad -= LoadGameSessionFromPlayer;
        SaveManager.onStartGameSessionLoaded -= LoadPlayers;
        SaveManager.onMultiplePlayersLoaded -= CallCorutineForGettingLoadedPlayers;
    }
    private void OnDestroy()
    {
        SaveManager.onPlayerLoad -= LoadGameSessionFromPlayer;
        SaveManager.onStartGameSessionLoaded -= LoadPlayers;
        SaveManager.onMultiplePlayersLoaded -= CallCorutineForGettingLoadedPlayers;
    }
    void Start()
    {
        cardMenuPlOne = GameObject.Find("PlayerOneCardMenu");
        cardMenuPlTwo = GameObject.Find("PlayerTwoCardMenu");
        cardMenus = GameObject.Find("CardMenus");
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        placeCardInField = GameObject.Find("Canvas").GetComponent<PlaceCardInField>();
        playDataList = new List<PlayerInfoData>();
        LoadThisPlayer();
    }
    void LoadThisPlayer()
    {
        SaveManager.Instance.LoadPlayerDataFromFirebase();
    }
    public void LoadGameSessionFromPlayer()
    {
        Debug.Log("Stage1");
        SaveManager.Instance.LoadPlayerGameSession(SaveManager.Instance.PlayerData.inGameID);
        SaveManager.onPlayerLoad -= LoadGameSessionFromPlayer;
    }
    public void LoadPlayers(GameData gameData)
    {
        Debug.Log("Stage2");
        SaveManager.Instance.LoadPlayersInGameSessionFromFirebase(gameData.playerIDs);
        SaveManager.onStartGameSessionLoaded -= LoadPlayers;
    }
    void GetLoadedPlayers()
    {
        Debug.Log("Stage3");
        playDataList = SaveManager.Instance.GetPlayersDataForGameSession();
        SaveManager.onMultiplePlayersLoaded -= CallCorutineForGettingLoadedPlayers;
        AddPlayerOneDeck();
        AddPlayerTwoDeck();
    }
    void CallCorutineForGettingLoadedPlayers()
    {
        StartCoroutine(WaitToGetLoadedPlayer());
    }

    IEnumerator WaitToGetLoadedPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        GetLoadedPlayers();
    }
    void AddPlayerOneDeck()
    {
        player1 = playDataList[0];
        Debug.Log("Player1 Name: " + player1.Name);
        PlayerPrefs.SetString(SaveManager.PLAYER_ONE,player1.Name);

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

            if (SaveManager.Instance.gameData.playerTurn && player1.Name.Contains(SaveManager.Instance.GetCurrentPlayerName()))
            {
                cardMenuPlOne.SetActive(true);
            }
            else
            {
                cardMenuPlOne.SetActive(false);
            }
        }
    }
    private void AddPlayerTwoDeck()
    {
        player2 = playDataList[1];
        Debug.Log("Player2 Name: " + player2.Name);
        PlayerPrefs.SetString(SaveManager.PLAYER_TWO, player2.Name);
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

        if (!SaveManager.Instance.gameData.playerTurn && player2.Name.Contains(SaveManager.Instance.GetCurrentPlayerName()))
        {
            cardMenuPlTwo.SetActive(true);
        }
        else
        {
            cardMenuPlTwo.SetActive(false);
        }
    }
}

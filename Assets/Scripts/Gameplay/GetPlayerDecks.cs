using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        FireBaseUserAuthenticator.onDataBaseConnected += LoadThisPlayer;
        SaveManager.onPlayerLoad += LoadDecksOnAfterDatabaseConnected;
        SaveManager.onStartGameSessionLoaded += LoadPlayers;
        SaveManager.onMultiplePlayersLoaded += GetLoadedPlayers;
    }
    private void OnDisable()
    {
        FireBaseUserAuthenticator.onDataBaseConnected -= LoadThisPlayer;
        SaveManager.onPlayerLoad -= LoadDecksOnAfterDatabaseConnected;
        SaveManager.onStartGameSessionLoaded -= LoadPlayers;
        SaveManager.onMultiplePlayersLoaded -= GetLoadedPlayers;
    }
    private void OnDestroy()
    {
        FireBaseUserAuthenticator.onDataBaseConnected -= LoadThisPlayer;
        SaveManager.onPlayerLoad -= LoadDecksOnAfterDatabaseConnected;
        SaveManager.onStartGameSessionLoaded -= LoadPlayers;
        SaveManager.onMultiplePlayersLoaded -= GetLoadedPlayers;
    }
    void Start()
    {
        cardMenuPlOne = GameObject.Find("PlayerOneCardMenu");
        cardMenuPlTwo = GameObject.Find("PlayerTwoCardMenu");
        cardMenus = GameObject.Find("CardMenus");
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        placeCardInField = GameObject.Find("Canvas").GetComponent<PlaceCardInField>();
        playDataList = new List<PlayerInfoData>();
    }
    void LoadThisPlayer()
    {
        SaveManager.Instance.LoadPlayerDataFromFirebase(FireBaseUserAuthenticator.Instance.auth.CurrentUser.UserId);
    }
    void LoadDecksOnAfterDatabaseConnected()
    {
        Debug.Log("Kuck1");
        SaveManager.Instance.LoadPlayerGameSession(SaveManager.Instance.PlayerData.inGameID[0]);
    }
    void LoadPlayers(GameData gameData)
    {
        Debug.Log("Kuck2");
        SaveManager.Instance.LoadMultiplePlayerDataFromFirebase(gameData.playerIDs);
    }
    void GetLoadedPlayers()
    {
        Debug.Log("Kuck3");
        playDataList = SaveManager.Instance.GetPlayersDataForGameSession();
        AddPlayerOneDeck();
        AddPlayerTwoDeck();
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
        cardMenuPlTwo.SetActive(false);
    }
}

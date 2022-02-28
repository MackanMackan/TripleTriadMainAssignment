using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class ActiveMatchGameDataHandler : MonoBehaviour
{
    public List<GameObject> cardPlaceHolders;
    public Sprite RedFrame;
    public Sprite SilverFrame;


    string gameId;
    public static GameData activeGameData;
    CardPrefabGetter cardGetter;
    void Start()
    {
        cardGetter = GameObject.Find("CardPrefabGetter").GetComponent<CardPrefabGetter>();
        SaveManager.onStartGameSessionLoaded += SetThisMatchDatabaseListener;
        ChangePlayerTurn.onChangeTurn += SaveGameTurnToFireBase;
        MatchManager.onGameOver += CleanUpAtGameOver;
    }
    void SetThisMatchDatabaseListener(GameData gameData)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("games").Child(gameData.gameID).ValueChanged += LoadGameData;
        gameId = gameData.gameID;
        activeGameData = gameData;
        InitLoadGameData();
    }
    void LoadGameData(object sender, ValueChangedEventArgs args)
    {
        SaveManager.Instance.LoadPlayerGameSession(gameId);
    }
    void InitLoadGameData()
    {
        SaveManager.Instance.LoadPlayerGameSession(gameId);
        SaveManager.onStartGameSessionLoaded -= SetThisMatchDatabaseListener;
        SaveManager.onStartGameSessionLoaded += CallCoroutineForAddingCards;
    }
    void CallCoroutineForAddingCards(GameData gameData)
    {
        StartCoroutine(WaitBeforeAddingCardsToBoard(gameData));
    }
    IEnumerator WaitBeforeAddingCardsToBoard(GameData gameData)
    {
        yield return new WaitForSeconds(1);
        PutCardsFromGameDataOnBoard(gameData);
    }
    void PutCardsFromGameDataOnBoard(GameData gameData)
    {
        Sprite spriteFrame;
        //First remove all card from boards to not get overlapping cards
        foreach (var placeholder in cardPlaceHolders)
        {
            if (placeholder.transform.childCount > 1)
            {
                Destroy(placeholder.transform.GetChild(1).gameObject);
            }
        }
        //Add all the new cards added
        for (int i = 0; i < 9; i++)
        {
            if (gameData.cardsOnField != null && gameData.cardsOnField[i] != null && !gameData.cardsOnField[i].Equals(""))
            {
                string card = gameData.cardsOnField[i];
                string cardName = card.Substring(0, gameData.cardsOnField[i].IndexOf("."));
                string frame = card.Substring(gameData.cardsOnField[i].IndexOf(".") + 1);

                GameObject instanceCard = Instantiate(cardGetter.GetCard(cardName));
                instanceCard.transform.SetParent(cardPlaceHolders[i].transform);
                instanceCard.transform.localPosition = new Vector3(0, 0, 0);
                instanceCard.transform.localScale = new Vector3(4, 4, 4);
                if (frame.Equals("RedFrame"))
                {
                    spriteFrame = RedFrame;
                }
                else
                {
                    spriteFrame = SilverFrame;
                }
                instanceCard.transform.GetChild(2).GetComponent<Image>().sprite = spriteFrame;
            }
        }

    }
    public void SaveGameTurnToFireBase(bool playerTurn)
    {
        string cardAndFrame;
        for (int i = 0; i < 9; i++)
        {
            if (cardPlaceHolders[i].transform.childCount < 2)
            {
                activeGameData.cardsOnField[i] = "";
                continue;
            }
            //Puts the name of the card and which players frame in one string since its the most essential info for the game
            cardAndFrame = cardPlaceHolders[i].transform.GetChild(1).gameObject.GetComponent<CardSettings>().CardName + "." + cardPlaceHolders[i].transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite.name;
            activeGameData.cardsOnField[i] = cardAndFrame;
        }
        activeGameData.playerTurn = playerTurn;
        activeGameData.numberOfTurns++;
        SaveGameData();
    }
    void SaveGameData()
    {
        SaveManager.Instance.SaveGameSession(activeGameData, gameId);
    }
    void CleanUpAtGameOver()
    {
        GameData cleanData = new GameData();
        cleanData.gameID = activeGameData.gameID;
        cleanData.displayName = activeGameData.displayName;
        SaveManager.Instance.SetPlayerGameID("");
        SaveManager.Instance.SaveUserToFirebase();
        SaveManager.Instance.SaveGameSession(cleanData, gameId);
    }
}

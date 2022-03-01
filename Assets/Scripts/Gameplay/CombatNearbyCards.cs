using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatNearbyCards : MonoBehaviour
{
    [SerializeField]
    GameObject topFighter;
    [SerializeField]
    GameObject rightFighter;
    [SerializeField]
    GameObject bottomFighter;
    [SerializeField]
    GameObject leftFighter;

    CardSettings myCard;
    Sprite fightingPlayerFrame;
    Image enemyPlayerFrame;
    List<int> myCardValues;
    List<int> enemyCardsValues;
    public CardSettings MyCard { get => myCard; set => myCard = value; }

    void Start()
    {
        myCardValues = new List<int>();
        enemyCardsValues = new List<int>();
    }

    public void Fight()
    {
        GetFighters();
        AddMyCardValuesToList();
        GetfightingPlayerFrame();
        MatchManager.AddScoreWhenPlayingCard(fightingPlayerFrame);
        for (int i = 0; i < 4; i++)
        {
            if (enemyCardsValues[i] == 0)
            {
                continue;
            }
            if (myCardValues[i] > enemyCardsValues[i])
            {
                enemyPlayerFrame = ReturnFighter(i).transform.GetChild(2).GetComponent<Image>();
                if (!enemyPlayerFrame.sprite.name.Equals(fightingPlayerFrame.name))
                {
                    enemyPlayerFrame.sprite = fightingPlayerFrame;
                    MatchManager.ReportPlayerScore(fightingPlayerFrame);
                }
            }
        }

    }
    void GetfightingPlayerFrame()
    {
        fightingPlayerFrame = myCard.transform.GetChild(2).GetComponent<Image>().sprite;
    }
    void GetFighters()
    {
        GetTopFighter();
        GetRightFighter();
        GetBottomFighter();
        GetLeftFighter();
    }
    void GetTopFighter()
    {

        try
        {
            if(topFighter != null)
            {
                topFighter = topFighter.transform.GetChild(1).gameObject;
                enemyCardsValues.Add(topFighter.GetComponent<CardSettings>().card.bottomValue);
            }
            else
            {
                enemyCardsValues.Add(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            enemyCardsValues.Add(0);
        }
    }
    void GetRightFighter()
    {
        try
        {
            if(rightFighter != null) { 
                rightFighter = rightFighter.transform.GetChild(1).gameObject;
                enemyCardsValues.Add(rightFighter.GetComponent<CardSettings>().card.leftValue);
            }
            else
            {
                enemyCardsValues.Add(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            enemyCardsValues.Add(0);
        }
    }
    void GetBottomFighter()
    {
        try
        {
            if(bottomFighter != null)
            {
                bottomFighter = bottomFighter.transform.GetChild(1).gameObject;
                enemyCardsValues.Add(bottomFighter.GetComponent<CardSettings>().card.topValue);
            }
            else
            {
                enemyCardsValues.Add(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            enemyCardsValues.Add(0);
        }
    }
    void GetLeftFighter()
    {
        try
        {
            if(leftFighter != null)
            {
                leftFighter = leftFighter.transform.GetChild(1).gameObject;
                enemyCardsValues.Add(leftFighter.GetComponent<CardSettings>().card.rightValue);
            }
            else
            {
                enemyCardsValues.Add(0);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);

            enemyCardsValues.Add(0);
        }
    }
    void AddMyCardValuesToList()
    {
        myCardValues.Add(myCard.card.topValue);
        myCardValues.Add(myCard.card.rightValue);
        myCardValues.Add(myCard.card.bottomValue);
        myCardValues.Add(myCard.card.leftValue);
    }
    GameObject ReturnFighter(int fighterSpot)
    {
        switch (fighterSpot)
        {
            case 0: return topFighter;
            case 1: return rightFighter;
            case 2: return bottomFighter;
            case 3: return leftFighter;
            default: return null;
        }
    }
}

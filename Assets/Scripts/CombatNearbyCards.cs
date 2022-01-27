using System;
using System.Collections;
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

    // Update is called once per frame
    void Update()
    {

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
            topFighter = topFighter.transform.GetChild(1).gameObject;
            enemyCardsValues.Add(topFighter.GetComponent<CardSettings>().BottomValue);
        }
        catch (Exception)
        {
            enemyCardsValues.Add(0);
        }
    }
    void GetRightFighter()
    {
        try
        {
            rightFighter = rightFighter.transform.GetChild(1).gameObject;
            enemyCardsValues.Add(rightFighter.GetComponent<CardSettings>().LeftValue);
        }
        catch (Exception)
        {
            enemyCardsValues.Add(0);
        }
    }
    void GetBottomFighter()
    {
        try
        {
            bottomFighter = bottomFighter.transform.GetChild(1).gameObject;
            enemyCardsValues.Add(bottomFighter.GetComponent<CardSettings>().TopValue);
        }
        catch (Exception)
        {
            enemyCardsValues.Add(0);
        }
    }
    void GetLeftFighter()
    {
        try
        {
            leftFighter = leftFighter.transform.GetChild(1).gameObject;
            enemyCardsValues.Add(leftFighter.GetComponent<CardSettings>().RightValue);
        }
        catch (Exception)
        {
            enemyCardsValues.Add(0);
        }
    }
    void AddMyCardValuesToList()
    {
        myCardValues.Add(myCard.TopValue);
        myCardValues.Add(myCard.RightValue);
        myCardValues.Add(myCard.BottomValue);
        myCardValues.Add(myCard.LeftValue);
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

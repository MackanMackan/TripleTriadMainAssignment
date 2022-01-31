using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSettings : MonoBehaviour
{

    private int topValue;
    private int rightValue;
    private int bottomValue;
    private int leftValue;
    private int starLevel;

    private string cardName;
    private Image artWork;

    public Card card;
    public int TopValue { get => topValue; }
    public int LeftValue { get => leftValue; }
    public int RightValue { get => rightValue; }
    public int BottomValue { get => bottomValue; }
    public string CardName { get => cardName; }
    public int StarLevel { get => starLevel; }

    private void Start()
    {
        cardName = card.cardName;
        topValue = card.topValue;
        rightValue = card.rightValue;
        bottomValue = card.bottomValue;
        leftValue = card.leftValue;
        starLevel = card.starLevel;

        artWork = transform.GetChild(1).GetComponent<Image>();
        artWork.sprite = card.artWork;

        transform.GetChild(3).GetComponent<TMP_Text>().text = "" + topValue;
        transform.GetChild(4).GetComponent<TMP_Text>().text = "" + rightValue;
        transform.GetChild(5).GetComponent<TMP_Text>().text = "" + bottomValue;
        transform.GetChild(6).GetComponent<TMP_Text>().text = "" + leftValue;
    }
}

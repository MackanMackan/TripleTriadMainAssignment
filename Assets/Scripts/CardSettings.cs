using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSettings : MonoBehaviour
{
    [SerializeField]
    private int topValue;
    [SerializeField]
    private int rightValue;
    [SerializeField]
    private int bottomValue;
    [SerializeField]
    private int leftValue;
    [SerializeField]
    private int starLevel;
    [SerializeField]
    private string cardName;

    public int TopValue { get => topValue; }
    public int LeftValue { get => leftValue; }
    public int RightValue { get => rightValue; }
    public int BottomValue { get => bottomValue; }
    public string CardName { get => cardName; }
    public int StarLevel { get => starLevel; }

    private void Start()
    {
        transform.GetChild(3).GetComponent<TMP_Text>().text = ""+topValue;
        transform.GetChild(4).GetComponent<TMP_Text>().text = ""+rightValue;
        transform.GetChild(5).GetComponent<TMP_Text>().text = ""+bottomValue;
        transform.GetChild(6).GetComponent<TMP_Text>().text = ""+leftValue;
    }
}

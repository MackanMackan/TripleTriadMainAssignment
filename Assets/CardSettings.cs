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
    private int leftValue;
    [SerializeField]
    private int rightValue;
    [SerializeField]
    private int bottomValue;
    [SerializeField]
    private string cardName;

    public int TopValue { get => topValue; }
    public int LeftValue { get => leftValue; }
    public int RightValue { get => rightValue; }
    public int BottomValue { get => bottomValue; }
    public string CardName { get => cardName; }
}

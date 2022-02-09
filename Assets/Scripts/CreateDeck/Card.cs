using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;

    public int topValue;
    public int rightValue;
    public int bottomValue;
    public int leftValue;
    public int starLevel;

    public Sprite artWork;
}

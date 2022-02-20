using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabGetter : MonoBehaviour
{
    public List<GameObject> cardList;
    private List<GameObject> cardCompareList;

    private void Start()
    {
        cardCompareList = new List<GameObject>();
        foreach (GameObject card in cardList)
        {
            cardCompareList.Add(Instantiate(card, new Vector3(5000, 5000, 0), Quaternion.identity));
        }
    }
    public GameObject GetCard(string name)
    {
       foreach(GameObject card in cardList)
        {
            if (card.GetComponent<CardSettings>().card.cardName.Equals(name))
            {
                return card;
            }
        }
        return null;
    }
}

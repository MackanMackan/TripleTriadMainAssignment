using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabGetter : MonoBehaviour
{
    public List<GameObject> cardList;
    public enum CardNames { EricRod, EricBlo, BlackSage, WhiteSage, UltimateMarcus, Jesusca, GongusGubben, CancelledViktorius,Carly,SoapGloob}
    void Start()
    {
        cardList = new List<GameObject>();
    }

    public GameObject GetCard(string name)
    {
       foreach(GameObject card in cardList)
        {
            if (card.GetComponent<CardSettings>().CardName.Equals(name))
            {
                return card;
            }
        }
        return null;
    }
}

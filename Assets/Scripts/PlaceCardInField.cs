using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceCardInField : MonoBehaviour
{
    GameObject card;
    GameObject cardPlaceholder;
    bool cardChosen = false;

    public void ChooseCardToPlay(GameObject chosenCard)
    {
        card = chosenCard;
        cardChosen = true;
    }

    public void ChooseCardPlacement(GameObject cardPlaceholder)
    {
        this.cardPlaceholder = cardPlaceholder;
    }
    public void AddChosenCardToPlaceholder()
    {
        if (cardChosen)
        {
            card.transform.SetParent(cardPlaceholder.transform);
            card.transform.position = cardPlaceholder.transform.position;
            card.transform.localScale = new Vector3(4,4,0);
            card.GetComponent<Button>().onClick.RemoveAllListeners();
           //cardPlaceholder.SetActive(false);
            cardChosen = false;
        }
    }
}
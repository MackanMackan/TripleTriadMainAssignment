using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerHasDeck : MonoBehaviour
{
    public GameObject playBtn;
    void OnEnable()
    {
        CheckDeckNotEmpty();
    }
    void Start()
    {
        SaveManager.onPlayerLoad += CheckDeckNotEmpty;
    }
    public void CheckDeckNotEmpty() { 
        if(SaveManager.Instance.PlayerData.Deck.Count != 5)
        {
            playBtn.SetActive(false);
        }
        else
        {
            playBtn.SetActive(true);
        }
        SaveManager.onPlayerLoad -= CheckDeckNotEmpty;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddChallenger : MonoBehaviour
{
    [SerializeField]
    TMP_Text warningText;

    [SerializeField]
    GameObject challengeBlocker;

    TMP_InputField challenger;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetChallenger(TMP_InputField challenger)
    {
        warningText.text ="Searching for "+ challenger.text;
        this.challenger = challenger;
        Invoke(nameof(SearchForPayer),0.2f);
       
    }
    void SearchForPayer()
    {
        if (SaveManager.Instance.LoadPlayerDataFromJsonSlave(challenger.text).Name.Equals("Default"))
        {
            warningText.text = "Could not find " + challenger.text + "!";
            challengeBlocker.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString(SaveManager.PLAYER_TWO, challenger.text);
            warningText.text = challenger.text + " found!";
            challengeBlocker.SetActive(true);
        }
    }
}

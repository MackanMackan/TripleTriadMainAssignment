using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoveChooseCardMenu : MonoBehaviour
{
    [SerializeField]
    float duration;
    void Start()
    {
        transform.position = Camera.main.WorldToScreenPoint(new Vector2(-4,0));
    }

     public void MoveMenuToScreen()
    {
        transform.DOMoveX(Camera.main.WorldToScreenPoint(new Vector2(0, 0)).x, duration);
    }
     public void MoveMenuAwayFromScreen()
    {
        transform.DOMoveX(Camera.main.WorldToScreenPoint(new Vector2(-4, 0)).x, duration);
    }
}

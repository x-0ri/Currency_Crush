using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilePiece : MonoBehaviour
{
    public int currency_type;//0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    Image img;

    public void Initialize(int v, Point p, Sprite orb)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        currency_type = v;
        SetIndex(p);
        img.sprite = orb;


    }
    public void ResetPosition()
    {
        pos = new Vector2(-182 + (33 * index.x), -182 + (33 * index.y)); //nie wiem ale dziala
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }
    void UpdateName()
    { 
        transform.name = "Node [" + index.x + "," + index.y + "]";    
    }
}

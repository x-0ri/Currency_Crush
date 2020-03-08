using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TilePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int currency_type;   //0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;
    public Animation abyssalboomanim;   
    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    public bool updating;
    Image img;

    public void Initialize(int v, Point p, Sprite orb)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        currency_type = v;
        SetIndex(p);
        img.sprite = orb;


    } // coś z podmianą obrazka
    public void ResetPosition()
    {
        pos = new Vector2(-182 + (33 * index.x), -181 + (33 * index.y)); //nie wiem ale dziala
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
    } //Tworzenie nazwy obiektu w grze
    public bool UpdatePiece() // funkcja zwracająca true jeżeli element jest w ruchu i false jeżeli jest w miejscu
    {
        if (Vector2.Distance(rect.anchoredPosition, pos) > 1) // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to : (jest w ruchu)
        {
            MovePositionTo(pos);    // wywołaj funkcje moveposition z parametrem pos
            updating = true;        // zwróć wartość true dla zmiennej updating
            return true;            // funkcja zwraca true
        }
        else // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to : (nie jest w ruchu)
        {
            rect.anchoredPosition = pos;    //zachowaj pozycje
            updating = false;               // zwróć wartość false dla zmiennej updating
            return false;                   // funkcja zwraca false
        }
    }

    public void MovePositionTo(Vector2 moveto)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, moveto, 0.13f);
    }
    public void OnPointerDown(PointerEventData eventData) //MouseButtonDown handler
    {
        if (updating) return;
        
        if (GameBoard.UsingJeweller) // check if jewller's powerup is in use
        {
            if (this.currency_type != 13)
            { 
                GameBoard.JewellerPowerUpPointIndex = this.index; // transfer selected tiliepiecs index to gameboard for further usage
            } 
        }
        else
            MovePieces.instance.MovePiece(this);
    }
    public void OnPointerUp(PointerEventData eventData) //MouseButtonUp handler
    {
        GameBoard.usedMouse = true;
        MovePieces.instance.DropPiece(); //wywołaj funkcje droppiece
        //Debug.Log("Drop " + transform.name);
    }

}

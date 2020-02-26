using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TilePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int currency_type;   //0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;
    
    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;

    bool updating;
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
        if (Vector3.Distance(rect.anchoredPosition, pos) > 1) // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to :
        {
            MovePositionTo(pos);    // wywołaj funkcje moveposition z parametrem pos
            updating = true;        // zwróć wartość true dla zmiennej updating
            return true;            // funkcja zwraca true
        }
        else // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to :
        {
            rect.anchoredPosition = pos;    //zachowaj pozycje
            updating = false;               // zwróć wartość false dla zmiennej updating
            return false;                   // funkcja zwraca false
        }
    }
    public void MovePosition(Vector2 move)
    {
        //rect.anchoredPosition += Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 10f);
        rect.anchoredPosition += move * Time.deltaTime * 10f;
        //rect.anchoredPosition = move;
    }
    public void MovePositionTo(Vector2 moveto)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition,moveto,Time.deltaTime * 10f);
    }
    public void OnPointerDown(PointerEventData eventData) //MouseButtonDown handler
    {
        if (updating) return;
        //Debug.Log("Grab " + transform.name);
        MovePieces.instance.MovePiece(this);
    }
    public void OnPointerUp(PointerEventData eventData) //MouseButtonUp handler
    {
        MovePieces.instance.DropPiece(); //wywołaj funkcje droppiece
        //Debug.Log("Drop " + transform.name);
    }

 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TilePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int currency_type;   // 0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;
    public bool updating;
    Image img;


    [HideInInspector]
    public Vector2 pos;                         // vector of destination
    [HideInInspector]
    public RectTransform rect;    

    private Vector2 velocity;                    // speed vector of falling element
    private Vector2 move_direction;              // direction related vector
    private readonly float a = 0.45f;            // artificial gravitation acceleration constant. Subject to change if needed.
    private readonly float eps = 10f;            // "snapping constant" used to check if moving element is in required approximation. Subject to change if needed.

    public void Update()                        // for debug updates
    {
        if (!GameBoard.DEBUG)
        { 
            img.color = new Color(1, 1, 1, 1); 
            return;
        }

        if (GameBoard.DEBUG && !updating) 
        { 
            img.color = new Color(0, 1, 0, 1); 
            return; 
        }

        if (GameBoard.DEBUG && updating)
        { 
            img.color = new Color(1, 0, 0, 1); 
            return; 
        }           
    }
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
        if (Vector2.Distance(rect.anchoredPosition, pos) > eps) // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to : (jest w ruchu)
        {
            GravityInterpolation(pos);  // wywołaj funkcje moveposition z parametrem pos
            updating = true;            // return yrue for updating -> piece is moving
            return true;                // funkcja zwraca true
        }
        else // jeżeli długość wektora róznicy starej i nowej poycji jest > 1 to : (nie jest w ruchu)
        {
            rect.anchoredPosition = pos;    // keep old position
            updating = false;               // return false for updating -> piece does not move
            velocity.x = 0;
            velocity.y = 0;

            return false;                   
        }
    }

    public void MovePositionTo(Vector2 moveto)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, moveto, 0.13f);
    }

    public void GravityInterpolation(Vector2 moveto)                        //gravitation based interpolation, does not deaccelerate when closing in, unlike Lerp function
    {
        move_direction.x = 0;                                               // set direction parameters dependent on direction of movement.
        if (moveto.x - rect.anchoredPosition.x > 0) move_direction.x = 1;   // Normally there should be sin and cos values of angles,         
        if (moveto.x - rect.anchoredPosition.x < 0) move_direction.x = -1;  // but movement of elements in this game is always orthogonal

        move_direction.y = 0;
        if (moveto.y - rect.anchoredPosition.y > 0) move_direction.y = 1;
        if (moveto.y - rect.anchoredPosition.y < 0) move_direction.y = -1;

        if (velocity.x + a < eps) velocity.x += a * move_direction.x;       //  check is max velocity will be reached in next frame                                                                                    
        if (velocity.y + a < eps) velocity.y += a * move_direction.y;       //  if not, increase velocity by a
               
        rect.anchoredPosition += velocity;

    }
    public void OnPointerDown(PointerEventData eventData) //MouseButtonDown handler
    {
        if (!GameBoard.CheckIfBoardIsStatic()) return;
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    GameBoard game;

    TilePiece moving;
    Point NewIndex;
    Vector2 mouseStart; // utwórz nowy wektor ktory zapisze pozycje myszki przy kliknieciu

    readonly int board_size = 12;
    private void Awake()
    {
        instance = this;    
    }

    public void MovePiece(TilePiece piece)
    {
        if (moving != null) return;
        moving = piece;
        mouseStart = Input.mousePosition; //przy kliknieciu ustaw bazowy wektor
        //Debug.Log("Clicked mouse on position : " + mouseStart.x + " , " + mouseStart.y);
    }

    public void DropPiece()
    {
        GameBoard.ComboCounter = 0;
        if (moving == null) return; // jeżeli bool moving ma wartość null
        //Debug.Log("Dropped");
        if (!NewIndex.Equals(moving.index))
            game.FlipPieces(moving.index, NewIndex, true);
        else
            game.ResetPiece(moving); //wywołaj funkcje ResetPiece
        
        moving = null;
                   
    }

    public int CheckPositionOnBoard(Point CheckIfOnBorder)
    {
        /* 
        
        Returns in which area of the board grabbed piece is
              |                     |
            2 | 7 7 7 7 7 7 7 7 7 7 | 2
          ----|---------------------|----
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
            6 | 9 9 9 9 9 9 9 9 9 9 | 6
          ----|---------------------|----
            1 | 5 5 5 5 5 5 5 5 5 5 | 4
              |                     |

        */

        if (CheckIfOnBorder.x == 0 && CheckIfOnBorder.y == 0)
        {
            //Debug.Log("Area : 1");
            return 1;
        }
        else
        {
            if (CheckIfOnBorder.x == 0 && CheckIfOnBorder.y == board_size - 1)
            {
                //Debug.Log("Area : 2");
                return 2;
            }
            else
            {
                if (CheckIfOnBorder.x == board_size - 1 && CheckIfOnBorder.y == board_size - 1)
                {
                    //Debug.Log("Area : 3");
                    return 3;
                }
                else
                {
                    if (CheckIfOnBorder.x == board_size - 1 && CheckIfOnBorder.y == 0)
                    {
                        //Debug.Log("Area : 4");
                        return 4;
                    }
                    else
                    {
                        if (CheckIfOnBorder.x > 0 && CheckIfOnBorder.x < board_size - 1 && CheckIfOnBorder.y == 0)
                        {
                            //Debug.Log("Area : 5");
                            return 5;
                        }
                        else
                        {
                            if (CheckIfOnBorder.y > 0 && CheckIfOnBorder.y < board_size - 1 && CheckIfOnBorder.x == 0)
                            {
                                //Debug.Log("Area : 6");
                                return 6;
                            }
                            else
                            {
                                if (CheckIfOnBorder.x > 0 && CheckIfOnBorder.x < board_size - 1 && CheckIfOnBorder.y == board_size - 1)
                                {
                                    //Debug.Log("Area : 7");
                                    return 7;
                                }
                                else
                                {
                                    if (CheckIfOnBorder.y > 0 && CheckIfOnBorder.y < board_size - 1 && CheckIfOnBorder.x == board_size - 1)
                                    {
                                        //Debug.Log("Area : 8");
                                        return 8;
                                    }
                                    else
                                    {
                                        {
                                            //Debug.Log("Area : 9");
                                            return 9;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


    }//returns int value depending on which board area Point is in

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<GameBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving != null) // jeśli bool moving jest rózny on null 
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);      //inicjalizuj wektor kierunkowy 
            Vector2 ndir = dir.normalized;                                  //utwórz wektor jednostkowy z "dir"
            Vector2 adir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y)); //utwórz wektor o wartościach zawsze dodatnich

            NewIndex = Point.Clone(moving.index);   // skopiuj obecną wartość index do nowej zmiennej
            Point add = Point.Zero;                 // utworz nowy punkt o wspolrzednych (0,0)

            //uproszcznie wzglednej pozycji myszki wokol punktu odniesienia do GORA DOL PRAWO LEWO
            switch (CheckPositionOnBoard(NewIndex)) 
            {
                case 1:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : 0, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : 0)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 2:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : 0, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 0 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 3:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 0 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 0 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 4:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 0 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : 0)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 5:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : 0)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 6:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : 0, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 7:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 0 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 8:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 0 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
                case 9:
                    {
                        if (dir.magnitude > 16)
                        {
                            if (adir.x > adir.y) // jezeli w wektorze [x,y] : |x| > |y|
                                add = (new Point((ndir.x > 0) ? 1 : -1, 0)); //to zmienna "add" jest punktem (1 dla ndir.x > 0 oraz -1 dla ndir.x < 0 , 0)
                            else if (adir.y > adir.x) // jezeli w wektorze [x,y] : |x| < |y|
                                add = (new Point(0, (ndir.y > 0) ? 1 : -1)); //to zmienna "add" jest punktem (0, 1 dla ndir.y > 0 oraz -1 dla ndir.y < 0)
                        }//jeżeli dlugosc wektora kierunkowego jest > 16 to:
                        break;
                    }
            } // ogranicz tworzenie wektora kierunkowego w zależnoeści od pozycji na planszy
            
            NewIndex.Add_indirect(add); // dodaj do zmiennej typu Point NewIndex zmienną add, jeden z wariantów : (1,0), (-1,0), (0,1), (0,-1)
            Vector2 pos = game.GetPositionFromPoint(moving.index);
            //utwórz wektor[x,y] "pos" o wartosci pozycji obecnego elementu. 
            //GameBoard game -> GetPostiionFromPoint ( TilePiece moving -> index (Point)) 

            if (!NewIndex.Equals(moving.index))             //jezeli funkcja NewIndex typu Point zwroci true to :
            {
                pos += Point.Multiply(add, 10).ToVector();   //dodaj do zmiennej "pos" typu Vector2 nowy wektor kierunkowy o dlugosci 10
            }

            moving.MovePositionTo(pos); //uzyj funkcji MovePositionTo z parametrem pos typu Vector2 ze zmiennej moving typu TilePiece
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    [Header("UI Elements")]
    public Sprite[] Orbs;
    public RectTransform gameBoard; //nie wiem ale dziala  
    
    [Header("Prefabs")]
    public GameObject TilePiece; //cos z instancjonowaniem

    static readonly int board_size = 12; //inicjalizacja wielkości planszy
    Tile[,] Tile; //inicjalizacja matrycy elementów
    
    public static string[] Currencies = { "Orb_Of_Alteration", "Jewellers_Orb", "Orb_Of_Alchemy", "Orb_Of_Fusing", "Vaal_Orb", "Chaos_Orb", "Exalted_Orb", "Mirror_Of_Kalandra", };
    public static int amount_of_currency_types = Currencies.Length; // deklaracja wartosci ilosci typow currency
    public int[] Board_TileData = new int[board_size * board_size]; //utwórz reprezentacje jednowymiarowa matrycy do przechowywania wartości currency_type

    List<TilePiece> update;
    List<FlippedPieces> flipped;
    //List<Point> matched; // lista w której przechowywane będą punkty dopasowania

    // Start is called before the first frame update
    void Start()
    {
        update = new List<TilePiece>();
        flipped = new List<FlippedPieces>();

        InitializeBoard();
        VerifyBoardInitialization();
        InstantiateBoard();
    }
    void InitializeBoard() //inicjalizowanie planszy
    {
        Tile = new Tile[board_size, board_size]; //inicjalizacja tablicy 2D board
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile[x, y] = new Tile(Mathf.CeilToInt(Random.Range(0, amount_of_currency_types)), new Point(x, y));
                //utwórz matryce 2D o typie Tile i parametrach  (lowowe 0 - ilosc typow currency , zmienna typu Point o wspolrzednych x i y)
                Board_TileData[x + y * board_size] = Tile[x, y].currency_type; //wpisz wartosc currency_type do reprezentacji 1D

            }
        }
        Debug.Log("Initialization of the board : Success");
    }

    void VerifyBoardInitialization() //Weryfikacja planszy
    {
        List<int> remove;  
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {
                Point p = new Point(x, y);
                int val = GetCurrencyTypeAtPoint(p);

                remove = new List<int>();
                while (IsConnected(p, true).Count > 0)
                {
                    val = GetCurrencyTypeAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);

                    SetCurrencyTypeAtPoint(p, Mathf.CeilToInt(Random.Range(0, amount_of_currency_types)));
                }
            }
        }
        Debug.Log("Eliminating starting matches - completed");
    }

    public bool VerificationFindMatch3(int x, int y) //used for board verification
    {
        if (x >= 2 && Board_TileData[x + y * board_size] == Board_TileData[(x - 1) + (y * board_size)]
                   && Board_TileData[(x - 1) + (y * board_size)] == Board_TileData[(x - 2) + (y * board_size)]
        ||  y >= 2 && Board_TileData[x + y * board_size] == Board_TileData[x + ((y - 1) * board_size)]
                   && Board_TileData[x + ((y - 1) * board_size)] == Board_TileData[x + ((y - 2) * board_size)])
        //      po ludzku : tak dlugo jak x >= 2 i trzy kolejne pola są identyczne w poziomie
        //      lub :       tak długo jak y >= 2 i trzy kolejne pola są identyczne w pionie
        //                  wykonuj pętle :
        {
            return true;
        }

        else
            return false;
    }

    void InstantiateBoard() //instancjonowanie planszy
    {
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile tile = GetTileAtPoint(new Point(x, y));

                int val = tile.currency_type;
                GameObject t = Instantiate(TilePiece, gameBoard);                       //zinstancjonowanie nowego GameObject na podstawie oryginału
                TilePiece orb = t.GetComponent<TilePiece>();
                RectTransform rect = t.GetComponent<RectTransform>();                   //nie wiem ale dziala
                rect.anchoredPosition = new Vector2(-182 + (33 * x), -181 + (33 * y));  //nie wiem ale dziala
                orb.Initialize(val, new Point(x, y), Orbs[val]);                        //zmiana Sprite pojedynczego elementu
                tile.SetPiece(orb);                                                     //wygeneruj element planszy - brak tej linijki powoduje wartosc null w obiekcie

            }
        }

        Debug.Log("Instantiation of the board : Success");
    }

    public Vector2 GetPositionFromPoint(Point p)
    {
        return new Vector2(-182 + (33 * p.x), -181 + (33 * p.y)); //nie wiem ale dziala
    }

    // Update is called once per frame
    void Update()
    {
        List<TilePiece> finishedUpdating = new List<TilePiece>();   // utwórz listę elementów których uaktualnianie się zakończyło
        for (int i = 0; i < update.Count; i++)                     // pętla for powtarzająca się 
        {
            TilePiece piece = update[i];                           // utwórz zmienną będącą indeksem i z Listy updated
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece); // dodaj do listy finishedUpdating piece

        }
        //Debug.Log(finishedUpdating.Count);
        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            TilePiece piece = finishedUpdating[i];                  // utwórz zmienną typu TilePiece o wartości indeksu listy finishedupdating
            //Debug.Log(finishedUpdating[i]);
            FlippedPieces flip = GetFlipped(piece);                 //
            TilePiece flippedPiece = null;       

            List<Point> matched = IsConnected(piece.index, true);

            bool wasFlipped = (flip != null);

            if (wasFlipped) // jeżeli zamienione zostały elementy
            {
                flippedPiece = flip.GetOtherPiece(piece);
                AddPoints(ref matched, IsConnected(flippedPiece.index, true));
            }

            if (matched.Count == 0) // jeżeli nie stworzone zostało dopasowanie
            {
                if (wasFlipped) // jeśli zamienione zostały elementy
                FlipPieces(piece.index, flippedPiece.index, false); // nie zamieniaj pozycji elementów
            }

            else // jeżeli stworzone zostało dopasowanie
            {
                foreach (Point pnt in matched) // usuń dopasowane elementy
                {
                    Tile tile = GetTileAtPoint(pnt);
                    TilePiece tilepiece = tile.GetPiece();
                    if (tilepiece != null)
                    {
                        tilepiece.gameObject.SetActive(false);
                    }
                    tile.SetPiece(null);
                }
            }
            
            flipped.Remove(flip); //usuń element flip po zaktualizowaniu
            update.Remove(piece);
        }
    }

    FlippedPieces GetFlipped(TilePiece t)
    {
        FlippedPieces tile = null;
        for (int i = 0; i < flipped.Count; i++)
        {
            if (flipped[i].GetOtherPiece(t) != null)
            {
                tile = flipped[i];
                break;
            }
        }
        return tile;
    }

    public void ResetPiece(TilePiece tile)
    {
        tile.ResetPosition();
        update.Add(tile);
    }

    public void FlipPieces(Point origin, Point destination, bool main)
    {
        if (GetCurrencyTypeAtPoint(origin) < 0) return;

        Tile tileOne = GetTileAtPoint(origin);          //utwórz lokalną zmienną typu Tile -> trzymany na myszce
        TilePiece pieceOne = tileOne.GetPiece();        //utwórz lokalną zmienną typu TilePiece -> trzmany na myszce
               

            Tile tileTwo = GetTileAtPoint(destination);     //utwórz lokalną zmienną typu Tile -> element do zamiany
            TilePiece pieceTwo = tileTwo.GetPiece();        //utwórz lokalną zmienną typu TilePiece -> element do zamiany

            tileOne.SetPiece(pieceTwo);                     //zamień element pierwotny na docelowy
            tileTwo.SetPiece(pieceOne);                   //zamień element docelowy na pierwotny za pomocą przechowania

            if (main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);
        
            ResetPiece(pieceOne);
    }

    Tile GetTileAtPoint(Point p)
    {
        return Tile[p.x, p.y];
    }

    int GetCurrencyTypeAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= board_size || p.y < 0 || p.y >= board_size) return -1;
        return Tile[p.x, p.y].currency_type;
    }
    void SetCurrencyTypeAtPoint(Point p, int v)
    {
        Tile[p.x, p.y].currency_type = v;
    }

    List<Point> IsConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int currency_type = GetCurrencyTypeAtPoint(p);
        Point[] directions = //tablica kierunków do sprawdzania
        {
            Point.Up,       //góra
            Point.Right,    //prawo
            Point.Down,     //dół
            Point.Left      //lewo
        };

        foreach (Point dir in directions)           //Sprawdź czy istnieją 2 takie same lub więcej w kierunkach
        {
            List<Point> line = new List<Point>();   //utwórz listę zawierającą 

            int same = 0; // licznik wartości currency_type
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.Add(p, Point.Multiply(dir, i));
                if (GetCurrencyTypeAtPoint(check) == currency_type)
                {
                    line.Add(check);    //dodaj punkt do listy "połączonych"
                    same++;             //zwieksz licznik o 1
                }
            }

            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
                AddPoints(ref connected, line); //Add these points to the overarching connected list
        }//Checking if there is 2 or more same shapes in the directions

        for (int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.Add(p, directions[i]), Point.Add(p, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                if (GetCurrencyTypeAtPoint(next) == currency_type)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }//Checking if we are in the middle of two of the same shapes

        if (main) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, IsConnected(connected[i], false));
        }//Checks for other matches along the current match

        /* UNNESSASARY | REMOVE THIS!
        if (connected.Count > 0)
            connected.Add(p);
        */

        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add) // dla każdego punktu w liście add
        {
            bool doAdd = true; // stwórz nowy bool
            for (int i = 0; i < points.Count; i++) // petla for - ilość powtórzeń jest równa wielkości listy points
            {
                if (points[i].Equals(p)) // jeżeli punkt o indeksie i z listy "points" jest równy obecnemu punktowi "p" z listy add
                {
                    doAdd = false; // nie dodawaj punktów (do innej listy?)
                    break; //skończ pętle for
                }
            }

            if (doAdd) points.Add(p); // jeżeli doAdd = true -> dodaj punkty
        }
    }
}

[System.Serializable]
public class Tile
{ 
    public int currency_type;//0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;
    TilePiece piece;
    public Tile(int v, Point i)
    {
        currency_type = v;
        index = i;
    }

    public void SetPiece(TilePiece t)
    {
        piece = t;
        currency_type = (piece == null) ? 0 : piece.currency_type;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public TilePiece GetPiece()
    {
        return piece;
    }
}

[System.Serializable]

public class FlippedPieces
{
    public TilePiece origin;
    public TilePiece destination;

    public FlippedPieces(TilePiece o, TilePiece d)
    {
        origin = o; destination = d;
    }

    public TilePiece GetOtherPiece(TilePiece t)
    {
        if (t == origin)
            return destination;
        else if (t == destination)
            return origin;
        else 
            return null;
    }
    
}
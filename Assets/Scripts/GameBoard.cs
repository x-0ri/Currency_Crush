using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public RectTransform gameBoard; //nie wiem ale dziala    
    [Header("Prefabs")]
    public GameObject TilePiece; //cos z instancjonowaniem
    public Sprite[] Orbs;
    public TilePiece piece;
    
    Tile[,] Tile; //inicjalizacja matrycy elementów

    static readonly int width = 12; //inicjalizacja wartosci szerokosci planszy
    static readonly int height = 12; //inicjalizacja wartosci wysokosci planszy
    
    public static string[] Currencies = { "Orb_Of_Alteration", "Jewellers_Orb", "Orb_Of_Alchemy", "Orb_Of_Fusing", "Vaal_Orb" , "Chaos_Orb" , "Exalted_Orb", "Mirror_Of_Kalandra", };
    public static int amount_of_currency_types = Currencies.Length; // deklaracja wartosci ilosci typow currency
    public int[] Board_TileData = new int[width * height]; //utwórz reprezentacje jednowymiarowa matrycy

    List<TilePiece> update;

    // Start is called before the first frame update
    void Start()
    {
        update = new List<TilePiece>();
        InitializeBoard();
        VerifyBoardInitialization();
        InstantiateBoard();
    }
    void InitializeBoard() //inicjalizowanie planszy
    {
        Debug.Log("Initialization of the board : Starting");
        Tile = new Tile[width, height]; //inicjalizacja tablicy 2D board
        for (int x = 0; x < width; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < height; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile[x, y] = new Tile(Mathf.CeilToInt(Random.Range(0,amount_of_currency_types)), new Point(x, y));
                //utwórz matryce 2D o typie Tile i parametrach  (lowowe 0 - ilosc typow currency , zmienna typu Point o wspolrzednych x i y)
                Board_TileData[x + y * height] = Tile[x, y].currency_type; //wpisz wartosc currency_type do reprezentacji 1D
                
            }        
        }
        Debug.Log("Initialization of the board : Success");
    }

    void VerifyBoardInitialization() //Trojprzejsciowa weryfikacja planszy
    {
        for (int z = 0; z < 2; z++)
        {

            string[] Debugstring = { "1st", "2nd"}; //debug string
            Debug.Log("Initialized " + Debugstring[z] +" pass of board verification for eliminating starting matches ..."); //fancy debug yay

            for (int x = 0; x < width; x++) //podwojny for - obsluga matrycy 2D
            {
                for (int y = 0; y < height; y++) //podwojny for - obsluga matrycy 2D
                {
                    int for_x = x; //wziecie x z petli for jako nowej zmiennej, inaczej wypierdala cale unity
                    int for_y = y; //bo program nigdy nie wyjdze z petli for
                    while (for_x >= 2 && Board_TileData[x + y * height] == Board_TileData[(x - 1) + (y * height)]
                                      && Board_TileData[(x - 1) + (y * height)] == Board_TileData[(x - 2) + (y * height)]
                      ||   for_y >= 2 && Board_TileData[x + y * height] == Board_TileData[x + ((y - 1) * height)]
                                      && Board_TileData[x + ((y - 1) * height)] == Board_TileData[x + ((y - 2) * height)])
                    //      po ludzku : tak dlugo jak x >= 2 i trzy kolejne pola są identyczne w poziomie
                    //      lub :       tak długo jak y >= 2 i trzy kolejne pola są identyczne w pionie
                    //                  wykonuj pętle :
                    {
                        Tile[x, y].currency_type = Mathf.CeilToInt(Random.Range(0, amount_of_currency_types)); //generuj nowe pole
                        for_x = 0;
                        for_y = 0;
                    }
                }
            }
        }
        Debug.Log("Eliminating starting matches - completed");
    }

    void InstantiateBoard() //instancjonowanie planszy
    {
        Debug.Log("Instantiation of the board : Starting");
        for (int x = 0; x < width; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < height; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile tile = GetTileAtPoint(new Point(x, y));
                int val = tile.currency_type;

                GameObject t = Instantiate(TilePiece, gameBoard); //zinstancjonowanie nowego GameObject na podstawie oryginału
                TilePiece orb = t.GetComponent<TilePiece>();
                RectTransform rect = t.GetComponent<RectTransform>(); //nie wiem ale dziala
                rect.anchoredPosition = new Vector2(-182 + (33 * x), -181 + (33 * y)); //nie wiem ale dziala
                orb.Initialize(val, new Point(x, y), Orbs[val]); //zmiana Sprite pojedynczego elementu
                tile.SetPiece(orb);

            }
        }

        Debug.Log("Instantiation of the board : Success");
    }

    public Vector2 GetPositionFromPoint(Point p)
    {
        return new Vector2(-182 + (33 * p.x), -182 + (33 * p.y)); //nie wiem ale dziala
    }

    // Update is called once per frame
    void Update()
    {
        List<TilePiece> finishedUpdating = new List<TilePiece>();
        for (int i = 0; i < update.Count; i++)
        {
            TilePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);
        }
        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            TilePiece piece = finishedUpdating[i];
            update.Remove(piece);
        }
    }

    public void ResetPiece(TilePiece piece)
    {
        piece.ResetPosition();
        piece.flipped = null;
        update.Add(piece);
    }

    public void FlipPieces(Point origin, Point destination)
    {

        Tile tileOne = GetTileAtPoint(origin);          //utwórz lokalną zmienną typu Tile -> trzymany na myszce
        TilePiece pieceOne = tileOne.GetPiece();        //utwórz lokalną zmienną typu TilePiece -> trzmany na myszce
        Debug.Log(tileOne);
        Debug.Log(pieceOne);

        Tile tileTwo = GetTileAtPoint(destination);     //utwórz lokalną zmienną typu Tile -> element do zamiany
        TilePiece pieceTwo = tileTwo.GetPiece();        //utwórz lokalną zmienną typu TilePiece -> element do zamiany
        Debug.Log(tileTwo);
        Debug.Log(pieceTwo);

        //Tile tile_temp = GetTileAtPoint(origin);        //utwórz lokalną zmienną typu Tile -> przechowanie pierwotnego elementu
        //TilePiece piece_temp = tile_temp.GetPiece();    //utwórz lokalną zmienną typu TilePiece -> przechowanie pierwotnego elementu
        //Debug.Log(tile_temp);
        //Debug.Log(piece_temp);

        //tile_temp.SetPiece(pieceOne);                   //przechowaj pierwotny element trzymany na myszce
        tileOne.SetPiece(pieceTwo);                     //zamień element pierwotny na docelowy
        tileTwo.SetPiece(pieceOne);                   //zamień element docelowy na pierwotny za pomocą przechowania

        Debug.Log(pieceOne);
        Debug.Log(pieceTwo);

        pieceOne.flipped = pieceTwo;
        pieceTwo.flipped = pieceOne;

        update.Add(pieceOne);
        update.Add(pieceTwo); 
    }

    Tile GetTileAtPoint(Point p)
    {
        return Tile[p.x, p.y];
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return Tile[p.x, p.y].currency_type;
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
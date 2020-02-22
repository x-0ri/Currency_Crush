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

    static int width = 12; //inicjalizacja wartosci szerokosci planszy
    static int height = 12; //inicjalizacja wartosci wysokosci planszy
    Tile[,] Board; //inicjalizacja matrycy elementów
    public static string[] Currencies = { "Orb_Of_Alteration", "Jewellers_Orb", "Orb_Of_Alchemy", "Orb_Of_Fusing", "Vaal_Orb" , "Chaos_Orb" , "Exalted_Orb", "Mirror_Of_Kalandra", };
    public static int amount_of_currency_types = Currencies.Length; // deklaracja wartosci ilosci typow currency
    public int[] Board_TileData = new int[width * height]; //utwórz reprezentacje jednowymiarowa matrycy

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initialization of void Start() : Success");
        InitializeBoard();
        VerifyBoardInitialization();
        InstantiateBoard();
    }
    void InitializeBoard() //inicjalizowanie planszy
    {
        Debug.Log("Initialization of the board : Starting");
        Board = new Tile[width, height]; //inicjalizacja tablicy 2D board
        for (int x = 0; x < width; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < height; y++) //podwojny for - obsluga matrycy 2D
            {

                Board[x, y] = new Tile(Mathf.CeilToInt(Random.Range(0,amount_of_currency_types)), new Point(x, y));
                //utwórz matryce 2D o typie Tile i parametrach  (lowowe 0 - ilosc typow currency , zmienna typu Point o wspolrzednych x i y)
                Board_TileData[x + y * height] = Board[x, y].currency_type; //wpisz wartosc currency_type do reprezentacji 1D
                
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
                        Board[x, y].currency_type = Mathf.CeilToInt(Random.Range(0, amount_of_currency_types)); //generuj nowe pole
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
                int val = Board[x, y].currency_type;
                GameObject t = Instantiate(TilePiece, gameBoard); //zinstancjonowanie nowego GameObject na podstawie oryginału
                TilePiece orb = t.GetComponent<TilePiece>();
                RectTransform rect = t.GetComponent<RectTransform>(); //nie wiem ale dziala
                rect.anchoredPosition = new Vector2(-182 + (33 * x), -182 + (33 * y)); //nie wiem ale dziala
                orb.Initialize(val, new Point(x, y), Orbs[val]); //zmiana Sprite pojedynczego elementu
                //Debug.Log("Cell [" + x + "," + y + "]" + " parameters : Currency Type = " + Board[x, y].currency_type + " -> " + Currencies[Board[x, y].currency_type]); //Log for viewing tile type

            }
        }

        Debug.Log("Instantiation of the board : Success");
    }

    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Tile
{ 
    public int currency_type;//0 - x -> ta wartosc odpowiada za rodzaj currency
    public Point index;
    public Tile(int v, Point i)
    {
        currency_type = v;
        index = i;
    }
}
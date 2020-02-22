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

    int width = 12; //inicjalizacja wartosci szerokosci planszy
    int height = 12; //inicjalizacja wartosci wysokosci planszy
    Tile[,] Board; //inicjalizacja matrycy elementów
    public static string[] Currencies = { "Orb_Of_Alteration", "Jewellers_Orb", "Orb_Of_Alchemy", "Orb_Of_Fusing", "Vaal_Orb" , "Chaos_Orb" , "Exalted_Orb", "Mirror_Of_Kalandra", };
    public static int amount_of_currency_types = Currencies.Length; // deklaracja wartosci ilosci typow currency
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();
        InstantiateBoard();
    }
    void InitializeBoard() //inicjalizowanie planszy
    {
        Board = new Tile[width, height]; //inicjalizacja tablicy 2D board
        for (int x = 0; x < width; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < height; y++) //podwojny for - obsluga matrycy 2D
            {

                Board[x, y] = new Tile(Mathf.CeilToInt(Random.Range(0,amount_of_currency_types)), new Point(x, y));
                //utwórz matryce 2D o typie Tile i parametrach  (lowowe 0 - ilosc typow currency , zmienna typu Point o wspolrzednych x i y)
                
            }        
        }
    }

    void InstantiateBoard() //instancjonowanie planszy
    {
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
                Debug.Log("Cell [" + x + "," + y + "]" + " parameters : Currency Type = " + Board[x, y].currency_type + " -> " + Currencies[Board[x, y].currency_type]); //Log for viewing tile type

            }
        }
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
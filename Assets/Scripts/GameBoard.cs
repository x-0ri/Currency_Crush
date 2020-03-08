using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    
    [Header("UI Elements")]
    public Sprite[] Orbs;
    public RectTransform gameBoard; //nie wiem ale dziala  
    public Text Score;
    public Text ComboBreaker;



    [Header("Jeweller PowerUp Handlers")]
    public Button JewellerButton;
    public Slider JewellerPowerUp;
    public Text JewellerPowerUpsAmountText;
    public Image JewellerCooldownImage;
    readonly int JewellerPowerUpRequirements = 200;
    [HideInInspector]
    int JewellerPowerUpProgress;
    [HideInInspector]
    int JewellerPowerUpsAmount;
    [HideInInspector]
    static public bool UsingJeweller = false;
    public static Point JewellerPowerUpPointIndex;
    [HideInInspector] 
    public float JewellerCooldown;



    [Header("Regret PowerUp Handlers")]
    public Button RegretButton;
    public Slider RegretPowerUp;
    public Text RegretPowerUpsAmountText;
    public Image RegretCooldownImage;
    readonly int RegretPowerUpRequirements = 720;
    [HideInInspector]
    int RegretPowerUpProgress;
    [HideInInspector]
    int RegretPowerUpsAmount;
    List<Tile> RegretIndexList;
    [HideInInspector] 
    public float RegretCooldown;




    [Header("Vaal PowerUp Handlers")]
    public Button VaalButton;
    public Slider VaalPowerUp;
    public Text VaalPowerUpsAmountText;
    public Image VaalCooldownImage;
    readonly int VaalPowerUpRequirements = 100;
    [HideInInspector]
    int VaalPowerUpProgress;
    [HideInInspector]
    int VaalPowerUpsAmount;
    public static List<Point> VaalPowerUpIndexes;
    int VaalPowerUpEvent = 666;
    [HideInInspector]
    public float VaalCooldown;



    [Header("Exalted PowerUp Handlers")]
    public Button ExaltedButton;
    public Slider ExaltedPowerUp;
    public Text ExaltedPowerUpsAmountText;
    public Image ExaltedCooldownImage;
    readonly int ExaltedPowerUpRequirements = 1200;
    [HideInInspector]
    public int ExaltedPowerUpProgress;
    [HideInInspector]
    int ExaltedPowerUpsAmount;
    public static List<Point> ExaltedPowerUpIndexes;
    int ExaltedPowerUpEvent = 0;
    [HideInInspector]
    public float ExaltedCooldown;


    static public bool usedMouse = false;


    [Header("Prefabs")]
    public GameObject Tile_Piece; //cos z instancjonowaniem

    static readonly int board_size = 12; //inicjalizacja wielkości planszy
    int[] fills;
    Tile[,] Tile; //inicjalizacja matrycy elementów

    public static int amount_of_currency_types = 13; // deklaracja wartosci ilosci typow currency

    List<TilePiece> update;
    List<FlippedPieces> flipped;
    List<TilePiece> dead;

    List<TilePiece> synchronizeboard;


    [HideInInspector]
    public int TotalDestroyedOrbsCounter;
    [HideInInspector]
    public int ComboCounter;

    public AudioSource Herald_Shatter;
    public AudioSource Abyssal_Explosion;
    public bool big_oomph;

    // Start is called before the first frame update
    void Start()
    {
        fills = new int[board_size];
        update = new List<TilePiece>();
        flipped = new List<FlippedPieces>();
        dead = new List<TilePiece>();

        synchronizeboard = new List<TilePiece>();

        JewellerPowerUpProgress = JewellerPowerUpRequirements;
        RegretPowerUpProgress = RegretPowerUpRequirements;
        VaalPowerUpProgress = VaalPowerUpRequirements;
        ExaltedPowerUpProgress = ExaltedPowerUpRequirements;


        JewellerButton.enabled = false;
        RegretButton.enabled = false;
        VaalButton.enabled = false;
        ExaltedButton.enabled = false;

        JewellerPowerUpPointIndex = null;
        VaalPowerUpIndexes = new List<Point>();

        ExaltedPowerUpIndexes = new List<Point>();

        RegretIndexList = new List<Tile>();

        JewellerCooldown = 1;
        JewellerCooldownImage.fillAmount = JewellerCooldown;

        RegretCooldown = 1;
        RegretCooldownImage.fillAmount = RegretCooldown;

        VaalCooldown = 1;
        VaalCooldownImage.fillAmount = VaalCooldown;

        ExaltedCooldown = 1;
        ExaltedCooldownImage.fillAmount = ExaltedCooldown;

        InitializeBoard();
        VerifyBoardInitialization();
        InstantiateBoard();

        big_oomph = false;

    }
    void InitializeBoard() //inicjalizowanie planszy
    {
        Tile = new Tile[board_size, board_size]; //inicjalizacja tablicy 2D board
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile[x, y] = new Tile(RollCurrencyType(), new Point(x, y));
                //utwórz matryce 2D o typie Tile i parametrach  (lowowe 0 - ilosc typow currency , zmienna typu Point o wspolrzednych x i y)

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
                int val; // = GetCurrencyTypeAtPoint(p);

                remove = new List<int>();
                while (IsConnected(p, true).Count > 0)
                {
                    val = GetCurrencyTypeAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);

                    SetCurrencyTypeAtPoint(p, RollCurrencyType());
                }
            }
        }
        Debug.Log("Eliminating starting matches - completed");
    }

    void InstantiateBoard() //instancjonowanie planszy
    {
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {

                Tile tile = GetTileAtPoint(new Point(x, y));

                int val = tile.currency_type;
                GameObject t = Instantiate(Tile_Piece, gameBoard);                      //zinstancjonowanie nowego GameObject na podstawie oryginału
                TilePiece orb = t.GetComponent<TilePiece>();
                RectTransform rect = t.GetComponent<RectTransform>();                   //nie wiem ale dziala
                rect.anchoredPosition = new Vector2(-182 + (33 * x), -181 + (33 * y));  //nie wiem ale dziala
                orb.Initialize(val, new Point(x, y), Orbs[val]);                        //zmiana Sprite pojedynczego elementu
                tile.SetPiece(orb);                                                     //wygeneruj element planszy - brak tej linijki powoduje wartosc null w obiekcie
                synchronizeboard.Add(orb);
            }
        }

        Debug.Log("Instantiation of the board : Success");
    }

    public void ApplyGravityToBoard()
    {
        
        for (int x = 0; x < board_size; x++) // idż od x = 0 do x = 11
        {
            for (int y = 0; y < board_size; y++) // idź od y = 0 do y = 11
            {
                Point p = new Point(x, y);
                Tile tile = GetTileAtPoint(p);
                int currency_type = GetCurrencyTypeAtPoint(p);

                if (currency_type != 0) continue; //if not a hole or corrupt move to the next

                for (int ny = (y + 1); ny <= board_size; ny++) // idź od y + 1 do board_size !?
                {
                    Point next = new Point(x, ny);
                    int nextCurrency_type = GetCurrencyTypeAtPoint(next);
                    if (nextCurrency_type == 0) // very important, idk why but if it gets commented update gets miscounted which results in NullException
                        continue;

                    if (nextCurrency_type != -1) // if next currency type is not hole
                    {
                        Tile got = GetTileAtPoint(next);
                        TilePiece piece = got.GetPiece();

                        //Set the hole
                        tile.SetPiece(piece);
                        update.Add(piece);

                        //Replace the hole
                        got.SetPiece(null);
                    }
                    else //Hit an end
                    {
                        //Debug.Log("Filling holes");
                        int newCurrencyType = RollCurrencyType();
                        TilePiece piece;// = null;
                        Point spawnPoint = new Point(x, ny + fills[x]);

                        if (dead.Count > 0)
                        {
                            TilePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            revived.rect.anchoredPosition = GetPositionFromPoint(spawnPoint);
                            piece = revived;

                            dead.RemoveAt(0);
                        }
                        else //should never be called, is here just in case
                        {
                            GameObject obj = Instantiate(Tile_Piece, gameBoard);
                            TilePiece t = obj.GetComponent<TilePiece>();
                            piece = t;
                        }
                        

                        piece.Initialize(newCurrencyType, p, Orbs[newCurrencyType]);
                        piece.rect.anchoredPosition = GetPositionFromPoint(spawnPoint);


                        Tile hole = GetTileAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;             

                        //fill the hole
                    }
                    break;
                }


            }
        }
    }

    public Vector2 GetPositionFromPoint(Point p)
    {
        return new Vector2(-182 + (33 * p.x), -181 + (33 * p.y)); //nie wiem ale dziala
    }

    // Update is called once per frame
    void Update()
    {
        if (UsingJeweller && JewellerPowerUpPointIndex != null) //add point from jewellers powerup to update, must be done to avoid null exception
        {
            Tile JewellerUsedTile = GetTileAtPoint(JewellerPowerUpPointIndex);
            TilePiece JewellerUsedTilePiece = JewellerUsedTile.GetPiece();
            update.Add(JewellerUsedTilePiece);

        }

        if (VaalPowerUpEvent == 0 || VaalPowerUpEvent == 1)
        {
            foreach (Point p in VaalPowerUpIndexes)
            {
                Tile VaalPowerUpUsedTile = GetTileAtPoint(p);
                TilePiece VaalPowerUpUsedTilePiece = VaalPowerUpUsedTile.GetPiece();
                update.Add(VaalPowerUpUsedTilePiece);
            }

        }

        if (ExaltedPowerUpEvent == 1)
        {
            big_oomph = true;
            foreach (Point p in ExaltedPowerUpIndexes)
            {
                Tile ExaltedPowerUpUsedTile = GetTileAtPoint(p);
                TilePiece ExaltedPowerUpUsedTilePiece = ExaltedPowerUpUsedTile.GetPiece();
                update.Add(ExaltedPowerUpUsedTilePiece);
            }

        }

        List<TilePiece> finishedUpdating = new List<TilePiece>();   // utwórz listę elementów których uaktualnianie się zakończyło
        List<Point> ToRemoveFromVaalPowerUpIndexes = new List<Point>();
        List<Point> ToRemoveFromExaltedPowerUpIndexes = new List<Point>();

        for (int i = 0; i < update.Count; i++)                     // pętla for powtarzająca się 
        {
            TilePiece piece = update[i];                           // utwórz zmienną będącą indeksem i z Listy updated
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece); // dodaj do listy finishedUpdating piece
        }
        //Debug.Log(finishedUpdating.Count);
        //ComboCounter = 0;

        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            if (CheckIfBoardIsStatic())
            {
            //NextRemoveDelayIsReached = false;
            //Debug.Log("Reset remove delay, state : " + NextRemoveDelayIsReached);
            
            TilePiece piece = finishedUpdating[i];                  // utwórz zmienną typu TilePiece o wartości indeksu listy finishedupdating     
            FlippedPieces flip = GetFlipped(piece);
            TilePiece flippedPiece = null;


            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] -1 , 0, board_size);


            List<Point> matched = IsConnected(piece.index, true);

            //add point from jewellers powerup to matched
            if (UsingJeweller && JewellerPowerUpPointIndex != null)
            {
                matched.Add(JewellerPowerUpPointIndex);
                UsingJeweller = false;
                Debug.Log("Used Jeweller PowerUp");
            }

            if (VaalPowerUpEvent == 0 || VaalPowerUpEvent == 1)
            {
                foreach (Point p in VaalPowerUpIndexes)
                {
                    matched.Add(p);
                    ToRemoveFromVaalPowerUpIndexes.Add(p);
                    VaalPowerUpEvent = 666;
                }

            }

            if (ExaltedPowerUpEvent == 1)
            {
                foreach (Point p in ExaltedPowerUpIndexes)
                {
                    matched.Add(p);
                    ToRemoveFromExaltedPowerUpIndexes.Add(p);
                    ExaltedPowerUpEvent = 0;
                }

            }

            List<Point> secondary_matched = new List<Point>();

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
                if (usedMouse)
                {
                    if (JewellerCooldown > 0)
                    {
                        JewellerCooldown -= 0.25f;
                        JewellerCooldownImage.fillAmount = JewellerCooldown;

                    }

                    if (RegretCooldown > 0)
                    {
                        RegretCooldown -= 0.01f;
                        RegretCooldownImage.fillAmount = RegretCooldown;

                    }

                    if (VaalCooldown > 0)
                    {
                        VaalCooldown -= 0.1f;
                        VaalCooldownImage.fillAmount = VaalCooldown;
                    }

                    if (ExaltedCooldown > 0)
                    {
                        ExaltedCooldown -= 0.05f;
                        ExaltedCooldownImage.fillAmount = ExaltedCooldown;

                    }
                    /*
                    Debug.Log(JewellerCooldown);
                    Debug.Log(RegretCooldown);
                    Debug.Log(VaalCooldown);                    
                    Debug.Log(ExaltedCooldown);
                    */

                    GameBoard.usedMouse = false;
                }

                ComboCounter += matched.Count;
                ComboCounter += secondary_matched.Count;

                List<TilePiece> Orb_Of_Alteration_Match = new List<TilePiece>();
                List<TilePiece> Jewellers_Orb_Match = new List<TilePiece>();
                List<TilePiece> Chromatic_Orb_Match = new List<TilePiece>();
                List<TilePiece> Orb_Of_Alchemy_Match = new List<TilePiece>();
                List<TilePiece> Orb_Of_Fusing_Match = new List<TilePiece>();
                List<TilePiece> Regal_Orb_Match = new List<TilePiece>();
                List<TilePiece> Orb_Of_Regret_Match = new List<TilePiece>();
                List<TilePiece> Vaal_Orb_Match = new List<TilePiece>();
                List<TilePiece> Chaos_Orb_Match = new List<TilePiece>();
                List<TilePiece> Divine_Orb_Match = new List<TilePiece>();
                List<TilePiece> Exalted_Orb_Match = new List<TilePiece>();
                List<TilePiece> Mirror_Of_Kalandra_Match = new List<TilePiece>();

                foreach (Point pnt in matched)                // usuń dopasowane elementy
                {

                    Tile tile = GetTileAtPoint(pnt);
                    TilePiece tilepiece = tile.GetPiece();

                    switch (tilepiece.currency_type)
                    {
                        case 1: //orbs of alteration
                            {
                                Orb_Of_Alteration_Match.Add(tilepiece);
                                JewellerPowerUpProgress--;
                                break;
                            }
                        case 2: //jewellers orb
                            {
                                Jewellers_Orb_Match.Add(tilepiece);
                                JewellerPowerUpProgress -= 2;

                                break;
                            }
                        case 3: // chromatic orb
                            {
                                Chromatic_Orb_Match.Add(tilepiece);
                                JewellerPowerUpProgress -= 6;

                                break;
                            }
                        case 4: // alchemy orb
                            {
                                Orb_Of_Alchemy_Match.Add(tilepiece);
                                RegretPowerUpProgress--;
                                break;

                            }
                        case 5: // orb of fusing
                            {
                                Orb_Of_Fusing_Match.Add(tilepiece);
                                JewellerPowerUpProgress -= 8;

                                break;
                            }
                        case 6: // regal orb
                            {
                                Regal_Orb_Match.Add(tilepiece);
                                ExaltedPowerUpProgress--;
                                break;
                            }
                        case 7: // orb of regret
                            {
                                Orb_Of_Regret_Match.Add(tilepiece);
                                RegretPowerUpProgress--;
                                break;
                            }
                        case 8: // vaal orb
                            {
                                Vaal_Orb_Match.Add(tilepiece);
                                VaalPowerUpProgress--;
                                break;
                            }
                        case 9: // chaos orb
                            {
                                Chaos_Orb_Match.Add(tilepiece);
                                ExaltedPowerUpProgress -= 2;
                                break;
                            }
                        case 10: // divine orb
                            {
                                Divine_Orb_Match.Add(tilepiece);
                                ExaltedPowerUpProgress -= 10;
                                break;
                            }
                        case 11: // Exalted orb
                            {
                                Exalted_Orb_Match.Add(tilepiece);
                                ExaltedPowerUpProgress -= 120;
                                break;
                            }
                        case 12: // Mirror Of Kalandra
                            {
                                Mirror_Of_Kalandra_Match.Add(tilepiece);
                                break;
                            }

                    }       // adding tilepiece to it's corresponding currency pool -> sorting removed currency

                    secondary_matched.AddRange(CreateSecondaryMatchList(Orb_Of_Alteration_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Jewellers_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Chromatic_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Orb_Of_Alchemy_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Orb_Of_Fusing_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Regal_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Orb_Of_Regret_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Vaal_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Chaos_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Divine_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Exalted_Orb_Match));
                    secondary_matched.AddRange(CreateSecondaryMatchList(Mirror_Of_Kalandra_Match));

                    if (tilepiece != null)
                    {
                        tilepiece.gameObject.SetActive(false);  // zamień elementy na nieaktywne
                        dead.Add(tilepiece);                    // dodaj do listy martwych elementów                
                        TotalDestroyedOrbsCounter++;            // zwieksz wynik
                        Score.text = TotalDestroyedOrbsCounter.ToString();  //zaktualizuj wynik na planszy
                        Play_Herald_Shatter();
                    }
                    tile.SetPiece(null);
                }

                foreach (Point pnt_sec in secondary_matched)
                {

                    Tile tile = GetTileAtPoint(pnt_sec);
                    TilePiece tilepiece = tile.GetPiece();

                    switch (tile.currency_type)
                    {
                        case 1: //orbs of alteration
                            {
                                JewellerPowerUpProgress--;
                                break;
                            }
                        case 2: //jewellers orb
                            {
                                JewellerPowerUpProgress -= 2;
                                break;
                            }
                        case 3: // chromatic orb
                            {
                                JewellerPowerUpProgress -= 6;
                                break;
                            }
                        case 4: // alchemy orb
                            {
                                RegretPowerUpProgress--;
                                break;
                            }
                        case 5: // orb of fusing
                            {
                                JewellerPowerUpProgress -= 8;
                                break;
                            }
                        case 6: // regal orb
                            {
                                break;
                            }
                        case 7: // orb of regret
                            {
                                RegretPowerUpProgress--;
                                break;
                            }
                        case 8: // vaal orb
                            {
                                VaalPowerUpProgress--;
                                break;
                            }
                        case 9: // chaos orb
                            {
                                break;
                            }
                        case 10: // divine orb
                            {
                                break;

                            }
                        case 11: // Exalted orb
                            {
                                break;
                            }
                        case 12: // Mirror Of Kalandra
                            {
                                break;
                            }

                    }       // adding tilepiece to it's corresponding currency pool -> sorting removed currency

                    if (tilepiece != null)
                    {
                        tilepiece.gameObject.SetActive(false);  // zamień elementy na nieaktywne
                        dead.Add(tilepiece);                    // dodaj do listy martwych elementów                
                        TotalDestroyedOrbsCounter++;            // zwieksz wynik
                        Score.text = TotalDestroyedOrbsCounter.ToString();  //zaktualizuj wynik na planszy
                        if (big_oomph)
                                Play_Abyssal_Explosion();
                    }
                    tile.SetPiece(null);

                }

                // Jewellers counters

                while (JewellerPowerUpProgress <= 0)
                {
                    int carry = JewellerPowerUpProgress * (-1);
                    JewellerPowerUpProgress = JewellerPowerUpRequirements - carry;
                    JewellerPowerUpsAmount++;
                }

                if (JewellerPowerUpsAmount != 0)
                {
                    JewellerButton.enabled = true;
                    JewellerPowerUpsAmountText.text = (JewellerPowerUpsAmount.ToString());
                }

                JewellerPowerUp.value = JewellerPowerUpProgress;

                // end


                // Regret counters

                while (RegretPowerUpProgress <= 0)
                {
                    int carry = RegretPowerUpProgress * (-1);
                    RegretPowerUpProgress = RegretPowerUpRequirements - carry;
                    RegretPowerUpsAmount++;
                }

                if (RegretPowerUpsAmount != 0)
                {
                    RegretButton.enabled = true;
                    RegretPowerUpsAmountText.text = (RegretPowerUpsAmount.ToString());
                }

                RegretPowerUp.value = RegretPowerUpProgress;

                //end

                // Vaal counters

                while (VaalPowerUpProgress <= 0)
                {
                    int carry = VaalPowerUpProgress * (-1);
                    VaalPowerUpProgress = VaalPowerUpRequirements - carry;
                    VaalPowerUpsAmount++;
                }

                if (VaalPowerUpsAmount != 0)
                {
                    VaalButton.enabled = true;
                    VaalPowerUpsAmountText.text = (VaalPowerUpsAmount.ToString());
                }

                VaalPowerUp.value = VaalPowerUpProgress;

                // end

                // Exalted counters

                while (ExaltedPowerUpProgress <= 0)
                {
                    int carry = ExaltedPowerUpProgress * (-1);
                    ExaltedPowerUpProgress = ExaltedPowerUpRequirements - carry;
                    ExaltedPowerUpsAmount++;
                }

                if (ExaltedPowerUpsAmount != 0)
                {
                    ExaltedButton.enabled = true;
                    ExaltedPowerUpsAmountText.text = (ExaltedPowerUpsAmount.ToString());
                }

                ExaltedPowerUp.value = ExaltedPowerUpProgress;

                // end


                ComboBreaker.text = ("+ " + ComboCounter.ToString());



                ApplyGravityToBoard();
            }

            flipped.Remove(flip); //usuń element flip po zaktualizowaniu
            update.Remove(piece);
        }
        }

        JewellerPowerUpPointIndex = null;

        foreach (Point p in ToRemoveFromVaalPowerUpIndexes)
        {
            VaalPowerUpIndexes.Remove(p);
        }

        foreach (Point p in ToRemoveFromExaltedPowerUpIndexes)
        {
            ExaltedPowerUpIndexes.Remove(p);
        }
        VaalPowerUpEvent = 666;
        ExaltedPowerUpEvent = 0;
        big_oomph = false;
    }
    private bool CheckFor4InLine(List<TilePiece> Currency_Match, int direction) // 0 - vertical check , 1 - horizontal check , returns true if 4 in one line
    {
        if (direction == 0)
        {
            if (Currency_Match[0].index.x == Currency_Match[1].index.x &&
                Currency_Match[1].index.x == Currency_Match[2].index.x &&
                Currency_Match[2].index.x == Currency_Match[3].index.x) // check if they are in vertical line
                return true;
            else
                return false;
        }

        else if (direction == 1)
        {
            if (Currency_Match[0].index.y == Currency_Match[1].index.y &&
                Currency_Match[1].index.y == Currency_Match[2].index.y &&
                Currency_Match[2].index.y == Currency_Match[3].index.y) // check if they are in horizontal line
                return true;
            else
                return false;
        }
        else return false;
    }
    private bool CheckFor5InLine(List<TilePiece> Currency_Match, int direction) // 0 - vertical check , 1 - horizontal check , returns true if 5 in one line
    {
        if (direction == 0)
        {
            if (Currency_Match[0].index.x == Currency_Match[1].index.x &&
                Currency_Match[1].index.x == Currency_Match[2].index.x &&
                Currency_Match[2].index.x == Currency_Match[3].index.x &&
                Currency_Match[3].index.x == Currency_Match[4].index.x) // check if they are in vertical line
                return true;
            else
                return false;
        }

        else if (direction == 1)
        {
            if (Currency_Match[0].index.y == Currency_Match[1].index.y &&
                Currency_Match[1].index.y == Currency_Match[2].index.y &&
                Currency_Match[2].index.y == Currency_Match[3].index.y &&
                Currency_Match[3].index.y == Currency_Match[4].index.y) // check if they are in horizontal line
                return true;
            else
                return false;
        }
        else return false;
    }
    private List<Point> CreateSecondaryMatchList(List<TilePiece> Currency_Match) // for example : takes Jewellers_Orb_Match and returns list of points to add to secondary match
    {
        List<Point> temp_secondary_match = new List<Point>();

        if (Currency_Match.Count > 3)
        {
            if (Currency_Match.Count == 4)
            {
                if (CheckFor4InLine(Currency_Match, 0)) // check if they are in vertical line
                {
                    for (int y = 0; y < board_size; y++)
                    {
                        int column = Currency_Match[0].index.x; // get column x to value
                        Point column_elements = new Point(column, y);    // get point to add to matched
                        temp_secondary_match.Add(column_elements);          // add points from column to secondary_matched list that will be removed later on
                    } // for whole column of matching x
                }

                if (CheckFor4InLine(Currency_Match, 1)) //check if they are in horizontal line
                {
                    for (int x = 0; x < board_size; x++)
                    {
                        int row = Currency_Match[0].index.y;   // get column x to value
                        Point row_elements = new Point(x, row);         // get point to add to matched
                        temp_secondary_match.Add(row_elements);            // add points from column to secondary_matched list that will be removed later on
                    } // for whole column of matching x
                }
            } // if more than 3, check for match of exactly 4
            else // if more than 4 check if they are in one line
            {
                if (CheckFor5InLine(Currency_Match, 0)) // check if there are 5 or more in match
                {
                    big_oomph = true;
                    for (int x = 0; x < board_size; x++)
                    {
                        for (int y = 0; y < board_size; y++)
                        {
                            Point compared_tile_index = new Point(x, y);
                            int compared_tile_currency_type = GetCurrencyTypeAtPoint(compared_tile_index);

                            if (Currency_Match[0].currency_type == compared_tile_currency_type)
                                temp_secondary_match.Add(compared_tile_index);

                        }
                    }
                }

                if (CheckFor5InLine(Currency_Match, 1)) // check if there are 5 or more in match
                {
                    big_oomph = true; 
                    for (int x = 0; x < board_size; x++)
                    {
                        for (int y = 0; y < board_size; y++)
                        {
                            Point compared_tile_index = new Point(x, y);
                            int compared_tile_currency_type = GetCurrencyTypeAtPoint(compared_tile_index);

                         if (Currency_Match[0].currency_type == compared_tile_currency_type)
                        temp_secondary_match.Add(compared_tile_index);

                        }
                    }
                }
            }
        }

        return temp_secondary_match;
    }

    public void PowerUp_Regret()
    {
        if (RegretPowerUpsAmount >= 1 && RegretCooldown <= 0)
        {
            DestroyBoard();
            InitializeBoard();
            VerifyBoardInitialization();
            InstantiateBoard();

            RegretPowerUpsAmount--;
            RegretCooldown = 1;
            RegretCooldownImage.fillAmount = RegretCooldown;

            if (RegretPowerUpsAmount == 0)
                RegretPowerUpsAmountText.text = ("");
            else
                RegretPowerUpsAmountText.text = RegretPowerUpsAmount.ToString();

        }

        RegretButton.enabled = false;
        RegretButton.enabled = true;
    }

    public void PowerUp_Jeweller()
    {
        if (JewellerPowerUpsAmount >= 1 && JewellerCooldown <= 0)
        {
            UsingJeweller = true;
            JewellerPowerUpsAmount--;

            JewellerCooldown = 1;
            JewellerCooldownImage.fillAmount = JewellerCooldown;

            if (JewellerPowerUpsAmount == 0)
                JewellerPowerUpsAmountText.text = ("");
            else
                JewellerPowerUpsAmountText.text = JewellerPowerUpsAmount.ToString();
        }
        JewellerButton.enabled = false;
        JewellerButton.enabled = true;
    }

    public void PowerUp_VaalOrb()
    {
        if (VaalPowerUpsAmount >= 1 && VaalCooldown <= 0)
        {
            float Previousamount = VaalCooldown;
            VaalCooldown = 1;
            VaalCooldownImage.fillAmount = Mathf.Lerp(Previousamount,VaalCooldown,Time.deltaTime);            

            ComboCounter = 0;
            ComboBreaker.text = (ComboCounter.ToString());

            int selectedevent = Mathf.CeilToInt(Random.Range(0, 100));

            if (selectedevent >= 0 && selectedevent < 25)
                selectedevent = 0;

            if (selectedevent >= 25 && selectedevent < 50)
                selectedevent = 1;

            if (selectedevent >= 50 && selectedevent < 75)
                selectedevent = 2;

            if (selectedevent >= 75 && selectedevent < 100)
                selectedevent = 3;

            Debug.Log("Selected event :" + selectedevent);

            VaalPowerUpEvent = selectedevent;

            switch (selectedevent)
            {
                case 0: // random destruction select two pieces of the type and remove them from board
                    {
                        Debug.Log("Executing event :" + selectedevent);
                        VaalPowerUpEvent = 0;

                        List<Point> selected_to_be_rekt = new List<Point>();

                        int SelectedCurrencyType1 = RollCurrencyType();
                        int SelectedCurrencyType2 = RollCurrencyType();

                        while (SelectedCurrencyType1 == SelectedCurrencyType2)
                        { 
                            SelectedCurrencyType2 = RollCurrencyType(); 
                        }

                        for (int x = 0; x < board_size; x++)
                        {
                            for (int y = 0; y < board_size; y++)
                            {
                                Point p = new Point(x, y);
                                Tile t = GetTileAtPoint(p);
                                if (t.currency_type == SelectedCurrencyType1 || t.currency_type == SelectedCurrencyType2)
                                    selected_to_be_rekt.Add(p);                            
                            }
                        }


                        Debug.Log("Removed pieces : " + selected_to_be_rekt.Count);
                        VaalPowerUpIndexes.AddRange(selected_to_be_rekt);
                        break;
                    }

                case 1: // select horizontal or vertical, then choose 
                    {
                        int choose_direction = Mathf.CeilToInt(Random.Range(0, 100));
                        var direction = (choose_direction >= 50) ? 0 : 1; // 0 - horizontal , 1 - vertical
                        if (direction == 0)
                        {
                            int choose_y = Mathf.CeilToInt(Random.Range(0, board_size - 2));

                            for (int x = 0; x < board_size; x++)
                            {
                                Point p1 = new Point(x, choose_y);
                                Point p2 = new Point(x, choose_y + 1);
                                Point p3 = new Point(x, choose_y + 2);

                                if (GetTileAtPoint(p1).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p1);

                                if (GetTileAtPoint(p2).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p2);

                                if (GetTileAtPoint(p2).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p3);
                            }
                        }// go horizontal

                        if (direction == 1)
                        {
                            int choose_x = Mathf.CeilToInt(Random.Range(0, board_size - 1));
                            for (int y = 0; y < board_size; y++)
                            {
                                Point p1 = new Point(choose_x, y);
                                Point p2 = new Point(choose_x + 1, y);
                                Point p3 = new Point(choose_x + 2, y);

                                if (GetTileAtPoint(p1).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p1);

                                if (GetTileAtPoint(p2).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p2);

                                if (GetTileAtPoint(p2).currency_type != 13)     //destroy only if not corrupted
                                    VaalPowerUpIndexes.Add(p3);
                            }
                        }// go vertical

                        break;
                    }

                case 2:
                    {
                        CorruptBoard();
                        break;
                    }

                case 3:
                    {
                        break;
                    }

            }

            Debug.Log("Vaal orb selected event: " + selectedevent);
            VaalPowerUpsAmount--;
            if (VaalPowerUpsAmount == 0)
                VaalPowerUpsAmountText.text = ("");
            else
                VaalPowerUpsAmountText.text = VaalPowerUpsAmount.ToString();

        }
        VaalButton.enabled = false;
        VaalButton.enabled = true;
    }

    public void PowerUp_Exalted()
    {
        if (ExaltedPowerUpsAmount >= 1 && ExaltedCooldown  <= 0)
        {
            ComboCounter = 0;
            ComboBreaker.text = (ComboCounter.ToString());
            ExaltedPowerUpsAmount--;
            ExaltedCooldown = 1;
            ExaltedCooldownImage.fillAmount = ExaltedCooldown;


            Debug.Log("Executing event :exalted orb");
            ExaltedPowerUpEvent = 1;

            List<Point> available = new List<Point>();
            List<Point> selected_to_be_rekt = new List<Point>();

            for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
            {
                for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
                {

                    Point p = new Point(x, y);
                    Tile tile = GetTileAtPoint(p);
                    if (tile.currency_type != 13)
                    {
                        available.Add(p);
                    }
                }
            }
            Debug.Log("Available : " + available.Count);
            int NewCounter = available.Count / 2;

            for (int z = 0; z < NewCounter; z++)
            {
                int selected = Mathf.CeilToInt(Random.Range(0, available.Count));   // select number from list position
                Point p = available[selected];                                      // create point from selected number
                selected_to_be_rekt.Add(p);                                         // add selected point to selected list
                available.Remove(p);                                                // remove same point from available list to avoid removing same piece twice

            }

            Debug.Log("Removed pieces : " + selected_to_be_rekt.Count);
            ExaltedPowerUpIndexes.AddRange(selected_to_be_rekt);
        }

        if (ExaltedPowerUpsAmount == 0)
            ExaltedPowerUpsAmountText.text = ("");
        else
            ExaltedPowerUpsAmountText.text = ExaltedPowerUpsAmount.ToString();

        ExaltedButton.enabled = false;
        ExaltedButton.enabled = true;
    }         
 
    void DestroyBoard() //zniszczenie planszy
    {
        for (int x = 0; x < board_size; x++) //podwojny for - obsluga matrycy 2D
        {
            for (int y = 0; y < board_size; y++) //podwojny for - obsluga matrycy 2D
            {
                Tile tile = GetTileAtPoint(new Point(x, y));
                TilePiece tilepiece = tile.GetPiece();
                RegretIndexList.Add(tile); 
                
                tilepiece.gameObject.SetActive(false);
                Destroy(tilepiece.gameObject);

            }
        }      
    }

    void CorruptBoard() // corrupt random 3x3 area on board
    {
        List<Point> CorruptedArea = new List<Point>();
        List<Point> ToSelectFrom = new List<Point>();   //create list of points to select from
        for (int x = 1; x < board_size - 1; x++)        //podwojny for - obsluga matrycy 2D
        {
            for (int y = 1; y < board_size - 1; y++)    //podwojny for - obsluga matrycy 2D
            {
                ToSelectFrom.Add(new Point(x, y));
            }
        }

        int SelectTile = Mathf.CeilToInt(Random.Range(0, ToSelectFrom.Count + 1)); //select point from list
        Point CorruptedAreaCenter = ToSelectFrom[SelectTile];

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                CorruptedArea.Add(new Point(x + CorruptedAreaCenter.x, y + CorruptedAreaCenter.y));
            }
        }

        foreach (Point p in CorruptedArea) 
        {
            Tile tile = GetTileAtPoint(p);
            TilePiece tilepiece = tile.GetPiece();
            tilepiece.gameObject.SetActive(false);
            Destroy(tilepiece.gameObject);

            tile = GetTileAtPoint(p);
            int val = 13;
            GameObject t = Instantiate(Tile_Piece, gameBoard);                      //zinstancjonowanie nowego GameObject na podstawie oryginału
            TilePiece orb = t.GetComponent<TilePiece>();
            RectTransform rect = t.GetComponent<RectTransform>();                   //nie wiem ale dziala
            rect.anchoredPosition = new Vector2(-182 + (33 * p.x), -181 + (33 * p.y));  //nie wiem ale dziala
            orb.Initialize(val, p , Orbs[val]);                        //zmiana Sprite pojedynczego elementu
            tile.SetPiece(orb);
        } //change pieces to corrupted tile


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

        if (GetCurrencyTypeAtPoint(destination) > 0 && GetCurrencyTypeAtPoint(destination) != 13 && GetCurrencyTypeAtPoint(origin) != 13)
        {
            Tile tileTwo = GetTileAtPoint(destination);     //utwórz lokalną zmienną typu Tile -> element do zamiany
            TilePiece pieceTwo = tileTwo.GetPiece();        //utwórz lokalną zmienną typu TilePiece -> element do zamiany

            tileOne.SetPiece(pieceTwo);                     //zamień element pierwotny na docelowy
            tileTwo.SetPiece(pieceOne);                   //zamień element docelowy na pierwotny za pomocą przechowania

            if (main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);

            
        }
        else        
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

    List<Point> IsConnected(Point p, bool main_call)
    {
        
        List<Point> connected = new List<Point>(); 
            
        int currency_type = GetCurrencyTypeAtPoint(p);
        if (currency_type == 13)
            return connected;


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

        if (main_call) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, IsConnected(connected[i], false));
        }//Checks for other matches along the current match
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

    int RollCurrencyType()
    {             
        int currency_type = Mathf.CeilToInt(Random.Range(1, amount_of_currency_types - 6)); 

        return currency_type;
    }

    public void Play_Herald_Shatter()
    {
        Herald_Shatter.PlayOneShot(Herald_Shatter.clip,0.18f); //stacking sound, sometimes goes LOUD AF
        //Herald_Shatter.Play(); non stacking sounds
    }

    public void Play_Abyssal_Explosion()
    {
        Abyssal_Explosion.PlayOneShot(Abyssal_Explosion.clip,0.13f);
    }

    public bool CheckIfBoardIsStatic()
    {
        int counter = 0;
        foreach (TilePiece t in synchronizeboard)
        {
            if (t.updating == false)
                counter++;
        }

        if (counter == 144)
            return true;
        else
            return false;
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
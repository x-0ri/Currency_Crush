using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public Slider ExpBar;    
    public Text LevelCounter;
    public AudioSource LevelUpSound;

    public static int Level;
    public static string LevelTxt;
    public static float Exp;
    public static float NextLvl;

    private readonly float Base_Exp = 20;       // amount of exp for next level - default 20
    private readonly float M = 1.26f;           // exponent base for level scaling
    private readonly float C = 4;               // exponential coefficient used in formula

    #region Weight modificators
    
    readonly float[] Milestone1 = new float[] { 0, 16,  16, 16, 16, 16, 32,     16,     8,      8,      4,  1, 0 };
    readonly float[] Milestone2 = new float[] { 0, 8,   8,  10, 16, 50, 40,     30,     20,     10,     8,  2, 0 };
    readonly float[] Milestone3 = new float[] { 0, 4,   4,  5,  16, 50, 100,    100,    100,    100,    16, 4, 0 };
    readonly float[] Milestone4 = new float[] { 0, 2,   2,  2,  10, 10, 100,    100,    100,    100,    32, 8, 0 };
    readonly float[] Milestone5 = new float[] { 0, 1,   1,  1,  7,  10, 110,    110,    110,    110,    64, 16, 0 };
    readonly float[] Milestone6 = new float[] { 0, 0,   0,  0,  5,  10, 90,     90,     90,     90,     90, 32, 1 };

    #endregion

    void Start()
    {
        if (Level > 1) { LevelCounter.text = Level.ToString(); return; }

        else
        {
            ExpBar.maxValue = Base_Exp;
            Exp = 0;
            Level = 1;
            LevelCounter.text = Level.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

        ExpBar.value = ExpBar.maxValue - Exp;
        if (ExpBar.value <= 0)
        {
            LevelUp();
        }   

    }

    void LevelUp()
    {
        LevelUpSound.PlayOneShot(LevelUpSound.clip, 0.30f); 
        Level++;                                                        // 1. Increase level
        LevelCounter.text = Level.ToString();
        float carry = Mathf.Abs(ExpBar.value);                          // 2. Carry any excess exp to next level      
        float coefficient = Mathf.Pow(M , (1 / (Level / C)));           // 3. Calculate coefficient for increase of required exp 
        float ToNextLevel = ExpBar.maxValue * coefficient;              // 4. Calculate next level req exp
        ExpBar.maxValue = ToNextLevel;                                  // 5. Set exp value on slider
        NextLvl = ToNextLevel;                                          // extracting static for saving purposes
        Exp = carry;                                                    // 6. Reset xp and add carry from previous level
        ModifySpawnRate_Levelup(Level);
    }

    void ModifySpawnRate_Levelup(int Level)
    {
        int Milestone = 1 + (Level / 10);

        switch (Milestone)
        {
            case 1:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone1[i];
                }
                break;
            }

            case 2:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone2[i];
                }
                break;
            }

            case 3:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone3[i];
                }
                break;
            }

            case 4:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone4[i];
                }
                break;
            }

            case 5:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone5[i];
                }
                break;
            }

            default:
            {
                for (int i = 1; i <= 12; i++)
                {
                    GameBoard.spawnweight[i] += Milestone6[i];
                }
                break;
            }

        }       

    }
}

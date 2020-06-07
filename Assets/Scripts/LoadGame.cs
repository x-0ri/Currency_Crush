using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public static void Load()
    {
        string prefix = "CC ";
        string toload; // used to set string in load        

        #region 1. Boardstate

        #region a) Tiles

        for (int x = 0; x < GameBoard.board_size; x++)
        {
            for (int y = 0; y < GameBoard.board_size; y++)
            {                           
                toload = prefix + "Tile : [" + x + "," + y + "]";
                GameBoard.LoadCurrency[x,y] = PlayerPrefs.GetInt(toload);

            }
        }

        #endregion

        #region b) Spawnweight

        for (int i = 0; i < GameBoard.spawnweight.Length; i++)
        {
            toload = prefix + "Spawnweight : " + i;
            GameBoard.spawnweight[i] = PlayerPrefs.GetFloat(toload);
        }

        #endregion

        #region c) Score

        toload = prefix + "Score";
        GameBoard.TotalDestroyedOrbsCounter = PlayerPrefs.GetInt(toload);

        toload = prefix + "Combocount";
        GameBoard.ComboCounter = PlayerPrefs.GetInt(toload);

        #endregion

        #endregion

        #region 2. Leveldata

        #region a) Level related

        toload = prefix + "Level";
        LevelSystem.Level = PlayerPrefs.GetInt(toload);

        toload = prefix + "Level text";
        LevelSystem.LevelTxt = PlayerPrefs.GetString(toload);

        #endregion

        #region b) Exp related

        toload = prefix + "Exp amount";
        LevelSystem.Exp = PlayerPrefs.GetFloat(toload);

        toload = prefix + "Max exp bar value";
        LevelSystem.NextLvl = PlayerPrefs.GetFloat(toload);

        #endregion

        #endregion

        #region 3. Powerup state

        #region a) Jeweller

        toload = prefix + "Jeweller Amount";
        GameBoard.JewellerPowerUpsAmount = PlayerPrefs.GetInt(toload);

        toload = prefix + "Jeweller Cooldown";
        GameBoard.JewellerCooldown = PlayerPrefs.GetFloat(toload);

        toload = prefix + "Jeweller Progress";
        GameBoard.JewellerPowerUpProgress = PlayerPrefs.GetInt(toload);

        #endregion

        #region b) Regret

        toload = prefix + "Regret Amount";
        GameBoard.RegretPowerUpsAmount = PlayerPrefs.GetInt(toload);

        toload = prefix + "Regret Cooldown";
        GameBoard.RegretCooldown = PlayerPrefs.GetFloat(toload);

        toload = prefix + "Regret Progress";
        GameBoard.RegretPowerUpProgress = PlayerPrefs.GetInt(toload);

        #endregion

        #region c) Vaal

        toload = prefix + "Vaal Amount";
        GameBoard.VaalPowerUpsAmount = PlayerPrefs.GetInt(toload);

        toload = prefix + "Vaal Cooldown";
        GameBoard.VaalCooldown = PlayerPrefs.GetFloat(toload);

        toload = prefix + "Vaal Progress";
        GameBoard.VaalPowerUpProgress = PlayerPrefs.GetInt(toload);

        #endregion

        #region d) Exalted

        toload = prefix + "Exalted Amount";
        GameBoard.ExaltedPowerUpsAmount = PlayerPrefs.GetInt(toload);

        toload = prefix + "Exalted Cooldown";
        GameBoard.ExaltedCooldown = PlayerPrefs.GetFloat(toload);

        toload = prefix + "Exalted Progress";
        GameBoard.ExaltedPowerUpProgress = PlayerPrefs.GetInt(toload);

        #endregion

        #endregion

    }
}

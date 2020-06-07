using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{     
    public static void Save()
    {
        string prefix = "CC ";
        string tosave; // used to set string in save        

        #region 1. Boardstate

        #region a) Tiles

        for (int x = 0; x < GameBoard.board_size; x++)
        {
            for (int y = 0; y < GameBoard.board_size; y++)
            {
                Point p = new Point(x, y);
                int c = GameBoard.LoadCurrencyTypeFromPoint(p);
                tosave = prefix + "Tile : [" + x + "," + y + "]";
                PlayerPrefs.SetInt(tosave, c);

            }
        }

        #endregion

        #region b) Spawnweight

        for (int i = 0; i < GameBoard.spawnweight.Length; i++)
        {
            float f = GameBoard.spawnweight[i];
            tosave = prefix + "Spawnweight : " + i;
            PlayerPrefs.SetFloat(tosave, f);
        }

        #endregion

        #region c) Score

        tosave = prefix + "Score";
        PlayerPrefs.SetInt(tosave, GameBoard.TotalDestroyedOrbsCounter);

        tosave = prefix + "Combocount";
        PlayerPrefs.SetInt(tosave, GameBoard.ComboCounter);

        #endregion

        #endregion

        #region 2. Leveldata

        #region a) Level related

         tosave = prefix + "Level";
         PlayerPrefs.SetInt(tosave, LevelSystem.Level);

         tosave = prefix + "Level text";
         PlayerPrefs.SetString(tosave, LevelSystem.Level.ToString());


        #endregion

        #region b) Exp related

        tosave = prefix + "Exp amount";
        PlayerPrefs.SetFloat(tosave, LevelSystem.Exp);

        tosave = prefix + "Max exp bar value";
        PlayerPrefs.SetFloat(tosave, LevelSystem.NextLvl);
        #endregion


        #endregion

        #region 3. Powerup state

        #region a) Jeweller

        tosave = prefix + "Jeweller Amount";
        PlayerPrefs.SetInt(tosave, GameBoard.JewellerPowerUpsAmount);

        tosave = prefix + "Jeweller Cooldown";
        PlayerPrefs.SetFloat(tosave, GameBoard.JewellerCooldown);

        tosave = prefix + "Jeweller Progress";
        PlayerPrefs.SetInt(tosave, GameBoard.JewellerPowerUpProgress);

        #endregion

        #region b) Regret

        tosave = prefix + "Regret Amount";
        PlayerPrefs.SetInt(tosave, GameBoard.RegretPowerUpsAmount);

        tosave = prefix + "Regret Cooldown";
        PlayerPrefs.SetFloat(tosave, GameBoard.RegretCooldown);

        tosave = prefix + "Regret Progress";
        PlayerPrefs.SetInt(tosave, GameBoard.RegretPowerUpProgress);

        #endregion

        #region c) Vaal

        tosave = prefix + "Vaal Amount";
        PlayerPrefs.SetInt(tosave, GameBoard.VaalPowerUpsAmount);

        tosave = prefix + "Vaal Cooldown";
        PlayerPrefs.SetFloat(tosave, GameBoard.VaalCooldown);

        tosave = prefix + "Vaal Progress";
        PlayerPrefs.SetInt(tosave, GameBoard.VaalPowerUpProgress);

        #endregion

        #region d) Exalted

        tosave = prefix + "Exalted Amount";
        PlayerPrefs.SetInt(tosave, GameBoard.ExaltedPowerUpsAmount);

        tosave = prefix + "Exalted Cooldown";
        PlayerPrefs.SetFloat(tosave, GameBoard.ExaltedCooldown);

        tosave = prefix + "Exalted Progress";
        PlayerPrefs.SetInt(tosave, GameBoard.ExaltedPowerUpProgress);

        #endregion

        #endregion

    }

}

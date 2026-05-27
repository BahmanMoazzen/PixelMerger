using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


public static class A
{
    public static class Tags
    {
        public static char PLUS_SIGN = '+';
        public static string OutOfStockTag = "Out Of Stock";
        public static string NotEnoughScoreTag = "Not Enough Score!";
        public static string PurchaseFailedTag = "Purchase Failed!";
        public static string PurchaseSuccessTag = "Purchase Succeeded!";
        public static string ShopIsNotReadyTag = "Shop is not ready!";
        public static string BuyOneTag = "You do not have enough to activate. Buy one?";
        public static string BuyShopItemTag = "You do not have enough <b>&&&</b> to activate. Buy one &&& in exchange of <b>$$$</b> score?";
        public static string BuyDeckTag = "To unlock <b>&&&</b> you need <b>$$$</b> score. Do you want to unlock now?";
        public static string IsLockedTag = "Locked";
        public static string CheckInternetConnection = "Check Your Internet Connection.";
        public static class LootLocker
        {
            public static string ShowRankSuccess = "Data Recieved Successfuly";
            public static string ShowRankFailed = "Data Recieve Failed!";
            public static string SubmitRankSuccess = "Score Submitted Successfuly";
            public static string SubmitRankFailed = "Score Submission Failed!";
        }
        public static string ScoreSaveTag()
        {
            return "BestScoreTag_" + ((int)Levels.DifficultyLevel).ToString();
        }


    }

    private static GameSettingInfo gameSetting;

    public static GameSettingInfo GameSetting
    {
        get
        {
            if (gameSetting == null)
            {
                gameSetting = Resources.Load<GameSettingInfo>("GameSettings");
            }
            return gameSetting;
        }
    }

    public static class Tools
    {
        public static string TousandSeprator(string iNumber, char iSeparator)
        {
            string TSS = string.Empty;
            int c = 0;
            for (int i = iNumber.Length - 1; i >= 0; i--)
            {
                TSS += iNumber[i];
                c++;
                if (c == 3)
                {
                    c = 0;
                    TSS += iSeparator;
                }
            }
            if (TSS[TSS.Length - 1].Equals(iSeparator))
            {
                TSS = TSS.Remove(TSS.Length - 1);
            }

            return _ReverseString(TSS);
        }
        public static string _ReverseString(string iString)
        {
            string ns = string.Empty;
            for (int i = iString.Length - 1; i >= 0; i--)
            {
                ns += iString[i];

            }

            return ns;
        }
        public static bool IntToBool(int iInput)
        {
            return iInput == 1 ? true : false;
        }
        public static int BoolToInt(bool iInput)
        {
            return iInput ? 1 : 0;
        }
        public static string ScoreToTitle(int iScore)
        {
            const int digits = 6;
            string scoreTitle = string.Empty;
            for (int i = 0; i < digits - iScore.ToString().Length; i++)
            {
                scoreTitle += "0";
            }

            return $"{scoreTitle}{iScore.ToString()}";
        }
    }
    public static class Levels
    {
        public static GameModes DifficultyLevel;
        const string THISROUNDSCORETAG = "ThisRoundScoreTag";
        public static int ThisRoundScore
        {
            get
            {
                return PlayerPrefs.GetInt(THISROUNDSCORETAG, 0);
            }
            set
            {
                PlayerPrefs.SetInt(THISROUNDSCORETAG, value);
            }
        }
        public static int BestScore
        {
            get
            {
                return PlayerPrefs.GetInt(Tags.ScoreSaveTag(), 0);
            }
            set
            {
                PlayerPrefs.SetInt(Tags.ScoreSaveTag(), value);
            }
        }
        public static bool SetBestScore(int iScore)
        {
            if (iScore > BestScore)
            {
                BestScore = iScore;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static class Pixels
    {
        const float scaleGrowthFactor = .5f;
        const float scaleBaseFactor = .5f;
        const float radiousBase = 0f;
        const float radiousFactor = .25f;
        const float massGrowthFactor = 5f;
        const float massBaseFactor = 1f;
        const int lowerPixelBound = 0;
        const int maxPixelBound = 4;
        public const float MAXLATENCY = 5f;

        public static int topPixelBound = 0;
        public static float PixelScale(int iOrder)
        {
            return (iOrder * scaleGrowthFactor) + scaleBaseFactor;
        }
        public static float PixelRadious(int iOrder)
        {
            return (iOrder * radiousFactor) + radiousBase;
        }
        public static int PixelScore(int iOrder)
        {
            return gameSetting.PixelMergeScores[iOrder];
        }

        public static float PixelMass(int iOrder)
        {
            return (iOrder * massGrowthFactor) + massBaseFactor;
        }

        public static GameObject PixelSkeleton
        {
            get
            {
                return A.GameSetting.PixelSkeleton;
            }
        }

        public static string PixelTag(int iOrder)
        {
            return "M" + iOrder.ToString();
        }

        public static PixelInfo RandomPixel()
        {
            int pixelOrder = UnityEngine.Random.Range(lowerPixelBound, Mathf.Min(topPixelBound, maxPixelBound));

            return GameSettings.CurrentDeckMergers[pixelOrder];
        }
        public static PixelInfo NextPixel(PixelInfo iMerger)
        {
            PixelInfo newPixel;
            if (iMerger.MergerOrder < GameSettings.CurrentDeckMergers.Count)
            {
                newPixel = GameSettings.CurrentDeckMergers[iMerger.MergerOrder];


            }
            else
                newPixel = null;
            if (topPixelBound < iMerger.MergerOrder + 1)
            {
                topPixelBound = iMerger.MergerOrder + 1;
                SoundManager._Instance._PlaySound(GameSounds.FirstMerge);
                BAHMANMessageBoxManager._INSTANCE._ShowMessage(newPixel.MergerName, Color.white, 3f, newPixel.MergerSprite);
            }
            
            return newPixel;
        }
        public static float PixelGizmoRadious(int iPixelOrder)
        {
            return gameSetting.PixelGizmoRadious[iPixelOrder];
        }
        public static string PixelNameFromFileName(string iFileName)
        {
            return iFileName.Substring(2, iFileName.Length - 2).Replace('-', ' ').Trim();
        }

        public static int PixelOrderFromFileName(string iMergerName)
        {
            string order = string.Empty;
            foreach (char c in iMergerName)
            {
                if (c == '-')
                {
                    break;
                }
                else
                {
                    order += c;
                }
            }
            return int.Parse(order);
        }

    }

    public static class GameSettings
    {
        public static string CURRENTDECKTAG = "CurrentDeckTag";

        public static int CurrentDeckPosition
        {
            get
            {
                return PlayerPrefs.GetInt(CURRENTDECKTAG, 0);
            }
            set
            {
                PlayerPrefs.SetInt(CURRENTDECKTAG, value);
            }
        }



        public static List<PixelInfo> CurrentDeckMergers
        {
            get
            {
                return A.GameSetting.AllDecks[CurrentDeckPosition].DeckPixels;
            }
        }
        public static Sprite InGameBackground
        {
            get
            {
                return A.GameSetting.AllDecks[CurrentDeckPosition].DeckGameBackGround;
            }
        }

    }


}
public enum GameModes { Easy, Normal, Insane }

[Serializable]
public struct GameSoundStructure
{
    public GameSounds Sound;
    public List<AudioClip> AudioClips;
}
public enum GameSounds { FirstMerge, Throw, Merge, GameOver, ButtomClicked, DeckSelect }

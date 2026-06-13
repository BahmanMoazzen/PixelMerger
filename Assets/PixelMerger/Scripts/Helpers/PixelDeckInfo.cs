using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDeck", order = 1, menuName = "Pixel Merger/New Deck")]
public class PixelDeckInfo : ScriptableObject
{
    /// <summary>
    /// name in deck select
    /// </summary>
    public string DeckName;
    /// <summary>
    /// the round bg image
    /// </summary>
    public Sprite DeckBackGround;
    /// <summary>
    /// the background of the game when the deck is selected
    /// </summary>
    public Sprite DeckGameBackGround;
    /// <summary>
    /// the display of deck in deck select
    /// </summary>
    public Sprite DeckIcon;
    /// <summary>
    /// all the pixels in deck
    /// </summary>
    public List<PixelInfo> DeckPixels;
    /// <summary>
    /// the weight increase step of the pixels in deck
    /// </summary>
    public float WeightIncreaseStep = .5f;
    /// <summary>
    /// the radious increase step of pixels in deck
    /// </summary>
    public float RadiousIncreaseStep = .5f;
    /// <summary>
    /// define if the deck is locked
    /// </summary>
    [SerializeField] bool isLock = false;
    /// <summary>
    /// The price of the deck in shop, if it is 0, it will be unlocked from the start
    /// </summary>
    public int Price = 0;


    public bool IsLocked
    {
        get
        {
            return A.Tools.IntToBool(PlayerPrefs.GetInt("PixelDeckInfo_IsLocked_" + DeckName, A.Tools.BoolToInt(isLock)));
        }

        set
        {
            PlayerPrefs.SetInt("PixelDeckInfo_IsLocked_" + DeckName, A.Tools.BoolToInt(value));
        }
    }
    public string GetSKUName()
    {
        
        return  $"PM_SKU_DECK_{DeckName.Replace(" ","")}";
    }
    /// <summary>
    /// read and create pixel name from pixel file name
    /// </summary>
    /// <param name="iMergerName">the name of the file</param>
    /// <returns>the trimmed name of pixel</returns>
    public string mergerName(string iMergerName)
    {
        return iMergerName.Substring(2, iMergerName.Length - 4).Replace('-', ' ');
    }
    /// <summary>
    /// converts the name of pixel to order in deck
    /// </summary>
    /// <param name="iMergerName">the name of pixel</param>
    /// <returns>the order in deck</returns>
    public int mergerOrder(string iMergerName)
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
    /// <summary>
    /// unlock the deck
    /// </summary>
    public void UnLock()
    {
        IsLocked = false;
    }
}


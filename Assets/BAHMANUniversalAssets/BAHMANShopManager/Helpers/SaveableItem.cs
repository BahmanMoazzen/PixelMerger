
using UnityEngine;
[CreateAssetMenu(fileName ="NewSaveableItem",menuName ="BAHMAN Unity Assets/Saveable Item",order =3)]
public class SaveableItem : ScriptableObject
{

    /// <summary>
    /// if value changed successfully this will trigger
    /// </summary>
    /// <param name="iSaveable">The saveable item that changed</param>
    /// <param name="iAmountChanged">The amount by which the value changed</param>
    public delegate void ValueChanged(SaveableItem iSaveable,int iAmountChanged);
    public static event ValueChanged OnValueChanged;

    public string _SKU;

    /// <summary>
    /// the image of saveable item
    /// </summary>
    public Sprite _Icon;
    /// <summary>
    /// the tag used for playerprefs to save
    /// </summary>
    public string _Tag;
    /// <summary>
    /// startup value of saveable object
    /// </summary>
    public int _DefaultAmount;


    /// <summary>
    /// current stock saved on disk
    /// </summary>
    public int _Stock
    {
        get
        {
            return PlayerPrefs.GetInt(_Tag, _DefaultAmount);
        }
        set
        {
            PlayerPrefs.SetInt(_Tag, value);
        }
    }
    

    /// <summary>
    /// change the amount saved on disk by iAmount and check if has stock --> if(_ChangeAmount(-1,true) {do the code becase have stock and reduced}else{item doesnt have enough stock}
    /// </summary>
    /// <param name="iAmount">The amount to change</param>
    /// <param name="iCheckZeroStock">Whether to check if the stock goes below zero</param>
    /// <returns>True if the amount was changed successfully, false otherwise</returns>
    public bool _ChangeAmount(int iAmount,bool iCheckZeroStock)
    {

        int currentAmount = _Stock;
        currentAmount = currentAmount + iAmount;
        if (iCheckZeroStock)
        {
            if (currentAmount < 0)
            {
                return false;
            }
            
        }
        _Stock = currentAmount;
        OnValueChanged?.Invoke(this,iAmount);
        return true;
    }
    public bool _ChangeAmount(int iAmount)
    {
        return _ChangeAmount(iAmount,true);
        
    }
    public void _ResetAmount()
    {
        _Stock = _DefaultAmount;
    }

    /// <summary>
    /// Gets a value indicating whether the item has stock available. check if item has stock more than zero
    /// </summary>
    public bool _HaveStock

    {
        get
        {
            if (_Stock > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool _HaveAmount(int iAmount)
    {
        if (_Stock >= iAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


using UnityEngine;
[CreateAssetMenu(fileName ="NewSaveableItem",menuName ="BAHMAN Unity Assets/Saveable Item",order =3)]
public class SaveableItem : ScriptableObject
{

    // if value changed successfully this will trigger
    public delegate void ValueChanged(SaveableItem iSaveable,int iAmountChanged);
    public static event ValueChanged OnValueChanged;

    public string _SKU;

    //the image of saveable item
    public Sprite _Icon;
    // the tag used for playerprefs to save
    public string _Tag;
    // startup value of saveable object
    public int _DefaultAmount;


    // current stock saved on disk
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
    

    // change the amount saved on disk by iAmount and check if has stock --> if(_ChangeAmount(-1,true) {do the code becase have stock and reduced}else{item doesnt have enough stock}
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

    public void _ResetAmount()
    {
        _Stock = _DefaultAmount;
    }

    // check if item has stock more than zero
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
}

using UnityEngine;
[CreateAssetMenu(fileName = "NewShopItem", menuName = "BAHMAN Unity Assets/Shop Item", order = 1)]
public class ShoptItemInfo : ScriptableObject
{
    const char TOUSANDSEPRATOR = ',';
    public string _ItemName, _ItemSKUID, _itemPrice;
    
    public string _ItemPrice
    {
        set { _itemPrice = value; }
        get 
        {

            if (IsTousandSeprated)
            {
                string TSS = string.Empty;
                int c = 0;
                for(int i=_itemPrice.Length-1; i >= 0; i--)
                {
                    TSS += _itemPrice[i];
                    c++;
                    if (c == 3)
                    {
                        c=0;
                        TSS += TOUSANDSEPRATOR;
                    }
                }
                if (TSS[TSS.Length - 1].Equals(TOUSANDSEPRATOR))
                {
                    TSS = TSS.Remove(TSS.Length-1);
                }
                return _ReverseString(TSS);
            }
            else
            {
                return _itemPrice;
            }

        }
    }
    public string _ReverseString(string iString)
    {
        string ns = string.Empty;
        for (int i = iString.Length - 1; i >= 0; i--)
        {
            ns += iString[i];

        }

        return ns;
    }
    public int _ItemChargeAmount;
    public SaveableItem _ItemInfo;
    public bool isToman= true;
    public bool IsTousandSeprated = true;

    
}

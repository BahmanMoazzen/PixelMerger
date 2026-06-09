using UnityEngine;
[CreateAssetMenu(fileName = "NewShopItem", menuName = "BAHMAN Unity Assets/Shop Item", order = 1)]
public class ShoptItemInfo : ScriptableObject
{
    const char TOUSANDSEPRATOR = ',';
    public string _ItemName, _ItemSKUID, _itemPrice;
    public int _intPrice
    {
        get
        {
            return int.Parse(_itemPrice);
        }
    }
    public string _ItemPrice
    {
        set { _itemPrice = value; }
        get
        {

            if (IsTousandSeprated)
            {
                
                return A.Tools.ThousandSeparator(_itemPrice);
            }
            else
            {
                return _itemPrice;
            }

        }
    }
    
    public int _ItemChargeAmount;
    public SaveableItem _ItemInfo;
    public bool isToman = true;
    public bool IsTousandSeprated = true;


}

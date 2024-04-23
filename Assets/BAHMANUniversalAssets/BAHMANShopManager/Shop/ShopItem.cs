using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public delegate void mouseClicked(string iSKUID);
    public static event mouseClicked OnMouseClicked;
    const string _RIALSUFFIX = "ريال";
    const string _TOMANSUFFIX = "تومن";
    ShoptItemInfo _ShopItemInfo;
    public ShoptItemInfo ItemInfo
    {
        set
        {
            _ShopItemInfo = value;
            _UpdateVisual();
        }
    }



    [SerializeField] Text _ItemNameText, _ItemPriceText,_ItemNumberProvides;
    [SerializeField] Image _ItemImage;

    
    private void _UpdateVisual()
    {
        _ItemImage.sprite = _ShopItemInfo._ItemInfo._Icon;
        _ItemNameText.text = _ShopItemInfo._ItemName;
        _ItemNumberProvides.text = _ShopItemInfo._ItemChargeAmount.ToString();

        if (_ShopItemInfo.isToman)
        {
            _ItemPriceText.text = string.Format("{0} {1}", _ShopItemInfo._ItemPrice, _TOMANSUFFIX);
        }
        else
        {
            _ItemPriceText.text = string.Format("{0} {1}", _ShopItemInfo._ItemPrice, _RIALSUFFIX);
        }
    }
    public void _ItemClicked()
    {
        OnMouseClicked?.Invoke(_ShopItemInfo._ItemSKUID);
        //print(_ShopItemInfo._ItemSKUID);
    }


}

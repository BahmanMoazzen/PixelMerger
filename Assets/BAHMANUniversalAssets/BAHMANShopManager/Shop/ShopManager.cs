/*
 * 
 * ShopManager Version 1.0
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// create a ShopManagerInfo ScriptableObject and then create ShopItemInfo for it

public class ShopManager : MonoBehaviour
{
    // fires when purchase is successful
    public delegate void purchaseSuccessful(ShoptItemInfo iPurchase);
    public static event purchaseSuccessful OnPurchaseSuccessfull;

    // fires when purchase is Fail
    public delegate void purchaseFail();
    public static event purchaseFail OnPurchaseFail;

    // if shop window closes this will fire
    public delegate void shopClosed();
    public static event shopClosed OnShopClosed;

    // instance to handle class
    public static ShopManager _INSTANCE;

    [SerializeField] string _shopKey;

    

    // this visibles shop panel should be fired within other codes to show shop panel
    public void _ShowShop()
    {
        //foreach(Image go in GetComponentsInChildren<Image>())
        //{
        //    go.enabled = true;
        //}
        //foreach(Text t in GetComponentsInChildren<Text>())
        //{
        //    t.enabled = true;
        //}
        _ShopPanel.SetActive(true);
    }
    public void _HideShop()
    {
        //foreach (Image go in GetComponentsInChildren<Image>())
        //{
        //    go.enabled = false;
        //}
        //foreach (Text t in GetComponentsInChildren<Text>())
        //{
        //    t.enabled = false;
        //}

        _ShopPanel.SetActive(false);
    }



    #region private

    // procedure to start purchase routine. it fires from ShopItem dont use on other codes.

    public void StartPurchase(string iSKUID)
    {
        _DebugDisplay.text = "Start Purchase";
        _currentSKU = iSKUID;
        _HideShop(); 
        StartCoroutine(_startPurchaseRoutin());
        
    }

    IEnumerator _startPurchaseRoutin()
    {
        _DebugDisplay.text = "INI Shop";

        yield return null;

    }

    // the setting of shop manager including shop items
    [SerializeField] ShopManagerInfo _ShopSetting;

    // the template of shop item ui
    [SerializeField] GameObject _ShopItemTemplate;

    // the panel of shop to show or hide
    [SerializeField] GameObject _ShopPanel;

    // the parent of _ShopItemTemplate
    [SerializeField] Transform _ShopItemParent;

    [Header("Debug Settings")]
    [SerializeField] bool _ShowDebugDisplay;
    [SerializeField] Text _DebugDisplay;
    

    private string _currentSKU;

    private void Awake()
    {
        if (_INSTANCE == null)
            _INSTANCE = this;
        DontDestroyOnLoad(this.gameObject);
        
    }
    void Start()
    {
        if(!_ShowDebugDisplay)  _DebugDisplay.enabled = false;
        StartCoroutine(_startupRoutine());
    }
    IEnumerator _startupRoutine()
    {
        _DebugDisplay.text = "Setting up Items";
        foreach (var item in _ShopSetting._AllShopItems)
        {
            GameObject go = Instantiate(_ShopItemTemplate, _ShopItemParent);
            go.GetComponent<ShopItem>().ItemInfo = item;
        }
        _HideShop();
        yield return 0;
    }
    void OnEnable()
    {
        ShopItem.OnMouseClicked += StartPurchase;
        
    }

    

    void OnDisable()
    {
        ShopItem.OnMouseClicked -= StartPurchase;
    }

    #endregion

}

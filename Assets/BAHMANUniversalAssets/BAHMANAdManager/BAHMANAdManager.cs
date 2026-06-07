using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class BAHMANAdManager : MonoBehaviour
{
    /// <summary>
    /// triggered when an ad is shown successfully, you can use this event to reward the player for watching the ad, for example, you can give the player some in-game currency or items as a reward for watching the ad
    /// </summary>
    public static event UnityAction OnAdSuccess;
    /// <summary>
    /// triggered when an ad failed to show, you can use this event to show a message to the player that the ad failed to show, and also to log the error message for debugging purposes
    /// </summary>
    public static event UnityAction OnAdFailed;
    /// <summary>
    /// Singleton instance of the BAHMANAdManager, you can use this instance to show interstitial and rewarded ads, and also to check if the ads are ready or not, for example, you can check if the rewarded ad is ready before showing it to the player, and also to show a loading screen while the ad is loading
    /// </summary>
    public static BAHMANAdManager Instance;

    /// <summary>
    /// a Text component to show the debug messages in the UI, you can assign a Text component to this field to show the debug messages in the UI, and also set the _provideDebug field to true, otherwise the debug messages will be shown in the console only
    /// </summary>
    [SerializeField] Text _debugText;
    /// <summary>
    /// whether to show the debug messages in the UI or not, if you want to show the debug messages in the UI, you need to assign a Text component to the _debugText field, and also set this field to true, otherwise the debug messages will be shown in the console only
    /// </summary>
    [SerializeField] bool _provideDebug;
    /// <summary>
    /// a GameObject to show a loading screen while the ad is loading, you can assign a GameObject to this field to show a loading screen while the ad is loading, and also to hide it when the ad is loaded successfully or failed to load
    /// </summary>
    [SerializeField] GameObject _loadScreen;
    UnityAction _adSuccessAction, _adFailAction, _purchaseSuccess, _purchaseFail;
    string _currentSKU;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }




#if UNITY_ANDROID
    string _adUnitId = "ca-app-pub-8025891393939268/7721488986";
    string _adAppId = "";
    string _adInterstitialID = "";
    string _adRewardedID = "";
    string _ShopKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCx83AZ5dczCKny3fFN9HDxsc53/qkrqRqVUfgV67G/QrKLnY7rsWceOQRH1odOPSzErSkkO3GHnrLKPbW62QxgJ791agkOpdvpAM9cZo7tBKJmX4fS0Ki+bqJgPuEaeA1LG/c2Yvtqpwunc7PFRZiwkECer0nb/MCclpOzTZn23wIDAQAB";

#elif UNITY_IPHONE
  string _adUnitId = "ca-app-pub-8025891393939268/5460365146";
  string _adAppId = "";
  string _adInterstitialID = "";
    string _adRewardedID = "";
    string _ShopKey = "";
#else
  string _adUnitId = "ca-app-pub-8025891393939268/5460365146";
  string _adAppId = "";
  string _adInterstitialID = "";
    string _adRewardedID = "";
    string _ShopKey = "";
#endif

    IEnumerator Start()
    {
        yield return null;
        if (_debugText != null)
        {
            if (!_provideDebug)
            {
                _debugText.gameObject.SetActive(false);
            }
            else
            {
                _debugText.gameObject.SetActive(true);
            }
        }
    }

    void _dlog(string iMsg)
    {
        if (_debugText != null)
        {
            _debugText.text = iMsg;
        }
        Debug.Log(iMsg);
    }

    public void _ShowInterstitialAd()
    {

    }
    public void _ShowInterstitialAd(UnityAction iSuccessAction, UnityAction iFailAction)
    {
        _adSuccessAction = iSuccessAction;
        _adFailAction = iFailAction;
        _ShowInterstitialAd();

    }

    public bool _IsRewardedAddReady()
    {

        return false;
    }
    public bool _IsInterstitialAddReady()
    {
        return false;
    }

    public void _ShowRewardedAd()
    {

    }


    public void _BuySKU(string iSKU, UnityAction iPurchaseSuccess, UnityAction iPurchaseFail)
    {
        _loadScreen.SetActive(true);
        //MyketIAB.init(_ShopKey);
        _currentSKU = iSKU;
        _purchaseFail = iPurchaseFail;
        _purchaseSuccess = iPurchaseSuccess;
    }
    public void _ShowRewardedAd(UnityAction iSuccessAction, UnityAction iFailAction)
    {
        _adSuccessAction = iSuccessAction;
        _adFailAction = iFailAction;
        _ShowRewardedAd();
    }
    void _finishBuyProcess()
    {
        _adSuccessAction = null;
        _adFailAction = null;
        _loadScreen.SetActive(false);
        //MyketIAB.unbindService();
    }

#if UNITY_ANDROID

    void OnEnable()
    {
        // Listen to all events for illustration purposes
        //IABEventManager.billingSupportedEvent += billingSupportedEvent;
        //IABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        //IABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        //IABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        //IABEventManager.querySkuDetailsSucceededEvent += querySkuDetailsSucceededEvent;
        //IABEventManager.querySkuDetailsFailedEvent += querySkuDetailsFailedEvent;
        //IABEventManager.queryPurchasesSucceededEvent += queryPurchasesSucceededEvent;
        //IABEventManager.queryPurchasesFailedEvent += queryPurchasesFailedEvent;
        //IABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        //IABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        //IABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        //IABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }



    void OnDisable()
    {
        // Remove all event handlers
        //IABEventManager.billingSupportedEvent -= billingSupportedEvent;
        //IABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        //IABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        //IABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        //IABEventManager.querySkuDetailsSucceededEvent -= querySkuDetailsSucceededEvent;
        //IABEventManager.querySkuDetailsFailedEvent -= querySkuDetailsFailedEvent;
        //IABEventManager.queryPurchasesSucceededEvent -= queryPurchasesSucceededEvent;
        //IABEventManager.queryPurchasesFailedEvent -= queryPurchasesFailedEvent;
        //IABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        //IABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        //IABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        //IABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }


    void billingSupportedEvent()
    {
        _dlog("billingSupportedEvent");
        //MyketIAB.queryPurchases();
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage("Myket ini success");
    }

    void billingNotSupportedEvent(string error)
    {
        _dlog("billingNotSupportedEvent: " + error);
        _purchaseFail?.Invoke();
        _finishBuyProcess();
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage("Myket ini fail");
    }

    //void queryInventorySucceededEvent(List<MyketPurchase> purchases, List<MyketSkuInfo> skus)
    //{
    //    _dlog(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));

    //    for (int i = 0; i < purchases.Count; ++i)
    //    {
    //        _dlog(purchases[i].ToString());
    //    }

    //    _dlog("-----------------------------");

    //    for (int i = 0; i < skus.Count; ++i)
    //    {
    //        _dlog(skus[i].ToString());
    //    }
    //}

    void queryInventoryFailedEvent(string error)
    {
        _dlog("queryInventoryFailedEvent: " + error);

    }

    //private void querySkuDetailsSucceededEvent(List<MyketSkuInfo> skus)
    //{
    //    _dlog(string.Format("querySkuDetailsSucceededEvent. total skus: {0}", skus.Count));

    //    for (int i = 0; i < skus.Count; ++i)
    //    {
    //        _dlog(skus[i].ToString());
    //    }
    //}

    private void querySkuDetailsFailedEvent(string error)
    {
        _dlog("querySkuDetailsFailedEvent: " + error);
    }

    //private void queryPurchasesSucceededEvent(List<MyketPurchase> purchases)
    //{
    //    _dlog(string.Format("queryPurchasesSucceededEvent. total purchases: {0}", purchases.Count));

    //    for (int i = 0; i < purchases.Count; ++i)
    //    {
    //        _dlog(purchases[i].ToString());
    //        if (purchases[i].ProductId == _currentSKU)
    //        {
    //            _purchaseSuccess?.Invoke();
    //            _finishBuyProcess();
    //            return;
    //        }

    //    }
    //    MyketIAB.purchaseProduct(_currentSKU);
    //}

    //private void queryPurchasesFailedEvent(string error)
    //{
    //    _dlog("queryPurchasesFailedEvent: " + error);
    //    MyketIAB.purchaseProduct(_currentSKU);
    //}

    //void purchaseSucceededEvent(MyketPurchase purchase)
    //{
    //    _dlog("purchaseSucceededEvent: " + purchase);
    //    //BAHMANMessageBoxManager._INSTANCE._ShowMessage("purchase success");
    //    MyketIAB.consumeProduct(purchase.ProductId);
    //    //BAHMANMessageBoxManager._INSTANCE._ShowMessage("starting consume");
    //}

    void purchaseFailedEvent(string error)
    {
        _dlog("purchaseFailedEvent: " + error);
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage("purchase failed");
        _purchaseFail?.Invoke();
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage("nulling actions");
        _finishBuyProcess();
    }

    //void consumePurchaseSucceededEvent(MyketPurchase purchase)
    //{
    //    _dlog("consumePurchaseSucceededEvent: " + purchase);
    //    //BAHMANMessageBoxManager._INSTANCE._ShowMessage("consume success");
    //    _purchaseSuccess?.Invoke();
    //    //BAHMANMessageBoxManager._INSTANCE._ShowMessage("nulling actions");
    //    _finishBuyProcess();

    //}
    void consumePurchaseFailedEvent(string error)
    {
        _dlog("consumePurchaseFailedEvent: " + error);
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage("consume failed");
        _purchaseFail?.Invoke();
        _finishBuyProcess();
    }

#endif

}

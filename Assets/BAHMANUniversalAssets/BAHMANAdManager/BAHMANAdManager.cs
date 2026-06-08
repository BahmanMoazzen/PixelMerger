using System.Collections;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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
    [SerializeField] bool _giveNoAd = false;
    /// <summary>
    /// whether to show the debug messages in the UI or not, if you want to show the debug messages in the UI, you need to assign a Text component to the _debugText field, and also set this field to true, otherwise the debug messages will be shown in the console only
    /// </summary>
    [SerializeField] bool _provideDebug = false;
    /// <summary>
    /// a GameObject to show a loading screen while the ad is loading, you can assign a GameObject to this field to show a loading screen while the ad is loading, and also to hide it when the ad is loaded successfully or failed to load
    /// </summary>
    [SerializeField] GameObject _loadScreen;
    UnityAction _adSuccessAction, _adFailAction, _purchaseSuccess, _purchaseFail;
    string _currentSKU;
    bool _isReadytoShowAd = false;

    private LevelPlayBannerAd _bannerAd;
    private LevelPlayInterstitialAd _interstitialAd;
    private LevelPlayRewardedAd _rewardedVideoAd;
    public bool IsReady { get { return _isReadytoShowAd; } }
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
    public void _ShowRewardedVideoDebug()
    {
        _showRewardedAd();
    }
    public void _ShowBannerAdDebug()
    {
        _showBannerAd();
    }
    public void _ShowInterstatialDebug()
    {
        _showInterstitialAd();
    }
    void _initializeAd()
    {
        _dlog("[LevelPlaySample] LevelPlay.ValidateIntegration");
        LevelPlay.ValidateIntegration();

        _dlog($"[LevelPlaySample] Unity version {LevelPlay.UnityVersion}");

        _dlog("[LevelPlaySample] Register initialization callbacks");
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        // SDK init
        _dlog("[LevelPlaySample] LevelPlay SDK initialization");
        LevelPlay.Init(AdConfig.AppKey);
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        _dlog($"[LevelPlaySample] SDK initialization failed with error: {error}");
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration configuration)
    {
        _dlog("[LevelPlaySample] SDK initialization completed successfully");
        _enableAds();
        _isReadytoShowAd = true;

    }

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
        _initializeAd();
    }

    void _dlog(string iMsg)
    {
        if (_debugText != null)
        {
            _debugText.text += "\n" + iMsg;
        }
        Debug.Log(iMsg);
    }
    /// <summary>
    /// this method is used to show the interstitial ad, you can call this method to show the interstitial ad, and also to pass the success and fail actions as parameters, for example, you can call this method when the player finishes a level, and you want to show an interstitial ad before showing the next level, and also to reward the player for watching the ad, or to show a message if the ad failed to show
    /// </summary>
    void _showInterstitialAd()
    {
        if (_giveNoAd)
        {
            _adFailAction?.Invoke();
            return;
        }
        _loadScreen.SetActive(true);
        _interstitialAd.LoadAd();
    }
    /// <summary>
    /// this method is used to show the interstitial ad, you can call this method to show the interstitial ad, and also to pass the success and fail actions as parameters, for example, you can call this method when the player finishes a level, and you want to show an interstitial ad before showing the next level, and also to reward the player for watching the ad, or to show a message if the ad failed to show
    /// </summary>
    /// <param name="iSuccessAction">action to run when the ad is shown successfully</param>
    /// <param name="iFailAction">action to run when the ad fails to show</param>
    public void _ShowInterstitialAd(UnityAction iSuccessAction, UnityAction iFailAction)
    {
        _adSuccessAction = iSuccessAction;
        _adFailAction = iFailAction;
        _showInterstitialAd();

    }

    /// <summary>
    /// this method is used to show the rewarded ad, you can call this method to show the rewarded ad, and also to pass the success and fail actions as parameters, for example, you can call this method when the player wants to watch a rewarded ad to get some in-game currency or items as a reward, and also to show a message if the ad failed to show
    /// </summary>
    void _showRewardedAd()
    {
        if (_giveNoAd)
        {
            _adFailAction?.Invoke();
            return;
        }
        _loadScreen.SetActive(true);
        _rewardedVideoAd.LoadAd();

    }
    /// <summary>
    /// this method is used to show the rewarded ad, you can call this method to show the rewarded ad, and also to pass the success and fail actions as parameters, for example, you can call this method when the player wants to watch a rewarded ad to get some in-game currency or items as a reward, and also to show a message if the ad failed to show
    /// </summary>
    /// <param name="iSuccessAction">action to run when the ad is shown successfully</param>
    /// <param name="iFailAction">action to run when the ad fails to show</param>
    public void _ShowRewardedAd(UnityAction iSuccessAction, UnityAction iFailAction)
    {
        _adSuccessAction = iSuccessAction;
        _adFailAction = iFailAction;
        _adFailAction += _closeLoadingPanel;
        _adSuccessAction += _closeLoadingPanel;
        _showRewardedAd();
    }
    /// <summary>
    /// this method is used to show the banner ad, you can call this method to show the banner ad, and also to pass the success and fail actions as parameters, for example, you can call this method when the player wants to see a banner ad at the bottom of the screen, and also to show a message if the ad failed to show
    /// </summary>
    /// <param name="iSuccessAction">action to run when the ad is shown successfully</param>
    /// <param name="iFailAction">action to run when the ad fails to show</param>
    public void _ShowBannerAd(UnityAction iSuccessAction, UnityAction iFailAction)
    {
        _adSuccessAction = iSuccessAction;
        _adFailAction = iFailAction;
        _adFailAction += _closeLoadingPanel;
        _adSuccessAction += _closeLoadingPanel;
        _showBannerAd();
    }
    /// <summary>
    /// starts to show the banner ad, you can call this method to start showing the banner ad, and also to pass the success
    /// </summary>
    private void _showBannerAd()
    {
        if(_giveNoAd)
        {
            _adFailAction?.Invoke();
            return;
        }
        _bannerAd.LoadAd();
    }
    void _enableAds()
    {
        // Register to ImpressionDataReadyEvent
        LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        // Create Rewarded Video object
        _rewardedVideoAd = new LevelPlayRewardedAd(AdConfig.RewardedVideoAdUnitId);

        // Register to Rewarded Video events
        _rewardedVideoAd.OnAdLoaded += RewardedVideoOnLoadedEvent;
        _rewardedVideoAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
        _rewardedVideoAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
        _rewardedVideoAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayedFailedEvent;
        _rewardedVideoAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
        _rewardedVideoAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
        _rewardedVideoAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
        _rewardedVideoAd.OnAdInfoChanged += RewardedVideoOnAdInfoChangedEvent;

        // Create Banner object
        _bannerAd = new LevelPlayBannerAd(AdConfig.BannerAdUnitId);

        // Register to Banner events
        _bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        _bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        _bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        _bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        _bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        _bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        _bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        _bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

        // Create Interstitial object
        _interstitialAd = new LevelPlayInterstitialAd(AdConfig.InterstitalAdUnitId);

        // Register to Interstitial events
        _interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        _interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        _interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        _interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        _interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        _interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        _interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;


    }
    void _closeLoadingPanel()
    {
        _loadScreen.SetActive(false);


    }
    #region Rewarded Ad Callbacks
    void RewardedVideoOnLoadedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnLoadedEvent With AdInfo: {adInfo}");
        _rewardedVideoAd.ShowAd();
    }

    void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdLoadFailedEvent With Error: {error}");
        _adFailAction?.Invoke();
    }

    void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedEvent With AdInfo: {adInfo}");

    }

    void RewardedVideoOnAdDisplayedFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedFailedEvent With AdInfo: {adInfo} and Error: {error}");
        _adFailAction?.Invoke();
    }

    void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdRewardedEvent With AdInfo: {adInfo} and Reward: {reward}");
        _adSuccessAction?.Invoke();
    }

    void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdClosedEvent With AdInfo: {adInfo}");
        //_adFailAction?.Invoke();
    }

    void RewardedVideoOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received RewardedVideoOnAdInfoChangedEvent With AdInfo {adInfo}");
    }
    #endregion

    #region Interstitial Ad Callbacks
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdLoadedEvent With AdInfo: {adInfo}");
        _interstitialAd.ShowAd();
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdLoadFailedEvent With Error: {error}");
        _adFailAction?.Invoke();
    }

    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdDisplayedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdDisplayFailedEvent With AdInfo: {adInfo} and Error: {error}");
        _adFailAction?.Invoke();
    }

    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdClickedEvent With AdInfo: {adInfo}");

    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdClosedEvent With AdInfo: {adInfo}");
        _adSuccessAction?.Invoke();
    }

    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received InterstitialOnAdInfoChangedEvent With AdInfo: {adInfo}");
    }
    #endregion


    #region Banner Ad Callbacks
    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdLoadedEvent With AdInfo: {adInfo}");
        _bannerAd.ShowAd();
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdLoadFailedEvent With Error: {error}");
        _adFailAction?.Invoke();
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdDisplayedEvent With AdInfo: {adInfo}");
        _adSuccessAction?.Invoke();
    }

    void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdDisplayFailedEvent With AdInfo: {adInfo} and Error: {error}");
        _adFailAction?.Invoke();
    }

    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdCollapsedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdLeftApplicationEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        _dlog($"[LevelPlaySample] Received BannerOnAdExpandedEvent With AdInfo: {adInfo}");
    }
    #endregion
    void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
    {
        _dlog($"[LevelPlaySample] Received ImpressionDataReadyEvent ToString(): {impressionData}");
        _dlog($"[LevelPlaySample] Received ImpressionDataReadyEvent allData: {impressionData.AllData}");
    }

    private void OnDisable()
    {

    }

}

using UnityEngine;
using UnityEngine.UI;

public class DeckButtomController : MonoBehaviour
{
    [SerializeField] Text _buttomTitle;
    [SerializeField] Image _buttomIcon;
    [SerializeField] GameObject _lockGameObject;
    PixelDeckInfo _deckInfo;
    int _deckOrder;
    SaveableItem _scoresItem;

    public void _CreateButtom(PixelDeckInfo iDeck, int iDeckOrder, SaveableItem iScores)
    {
        _deckInfo = iDeck;
        _deckOrder = iDeckOrder;
        _scoresItem = iScores;
        _buttomIcon.sprite = iDeck.DeckIcon;
        _buttomIcon.preserveAspect = true;
        _buttomTitle.text = iDeck.DeckName;
        //Debug.Log("Name:" + _deckInfo.DeckName + " Lock:" + _deckInfo.IsLocked);
        if (_deckInfo.IsLocked)
        {
            _lockGameObject.SetActive(true);

        }
        else
        {
            _lockGameObject.SetActive(false);
        }
    }
    void _unlockWithAd()
    {
        BAHMANAdManager._Instance._ShowRewardedAd(AdManager__OnAdSuccess, AdManager__OnAdFailed);

    }
    void _unlockWithPurchase()
    {
        if (_scoresItem._ChangeAmount(-_deckInfo.Price))
        {
            _perchaseSuccess();
        }
        else
        {
            _purchaseFailed();
        }
        //BAHMANAdManager._Instance._BuySKU(_deckInfo.GetSKUName(), _perchaseSuccess, _purchaseFailed);
    }
    void _perchaseSuccess()
    {
        AdManager__OnAdSuccess();
    }
    void _purchaseFailed()
    {
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.PurchaseFailedTag);
        BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.NotEnoughScoreTag);
    }
    public void _ButtomClicked()
    {
        if (_deckInfo.IsLocked)
        {
            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(A.Tags.IsLockedTag, A.Tags.BuyDeckTag.Replace("&&&", _deckInfo.DeckName).Replace("$$$", A.Tools.TousandSeprator(_deckInfo.Price.ToString(), ',')), _unlockWithPurchase);

        }
        else
        {
            A.GameSettings.CurrentDeckPosition = _deckOrder;
            SoundManager._Instance._PlaySound(GameSounds.DeckSelect);
            //AdManager._Instance.ShowInterstitialAd();
            BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.GameScene);
        }
    }
    private void AdManager__OnAdFailed()
    {

        BAHMANMessageBoxManager._INSTANCE?._ShowMessage(A.Tags.PurchaseFailedTag);
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.CheckInternetConnection);
    }
    private void AdManager__OnAdSuccess()
    {
        BAHMANMessageBoxManager._INSTANCE?._ShowMessage(A.Tags.PurchaseSuccessTag);
        _deckInfo.IsLocked = false;
        _lockGameObject.SetActive(false);

    }

}

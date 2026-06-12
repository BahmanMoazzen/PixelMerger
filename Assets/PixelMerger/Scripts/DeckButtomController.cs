using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class DeckButtomController : MonoBehaviour
{
    public static event UnityAction OnDeckSelected;
    [SerializeField] Text _buttomTitle;
    [SerializeField] Image _buttomIcon;
    [SerializeField] GameObject _lockGameObject;
    [SerializeField] Slider _unlockSlider;
    [SerializeField] TMP_Text _unlockPointText;
    [SerializeField] GameObject _unlockButton;
    PixelDeckInfo _deckInfo;
    int _deckOrder;

    public void _CreateButtom(PixelDeckInfo iDeck, int iDeckOrder)
    {
        _deckInfo = iDeck;
        _deckOrder = iDeckOrder;

        _buttomIcon.sprite = iDeck.DeckIcon;
        _buttomIcon.preserveAspect = true;
        _buttomTitle.text = iDeck.DeckName;
        //Debug.Log("Name:" + _deckInfo.DeckName + " Lock:" + _deckInfo.IsLocked);
        if (_deckInfo.IsLocked)
        {
            if (A.GameSetting.ScoreSavable._HaveAmount(_deckInfo.Price))
            {
                _unlockSlider.gameObject.SetActive(false);
                _unlockButton.SetActive(true);


            }
            else
            {
                _lockGameObject.SetActive(true);
                _unlockSlider.maxValue = _deckInfo.Price;
                _unlockSlider.value = A.GameSetting.ScoreSavable._Stock;
                _unlockPointText.text = A.Tools.ThousandSeparator(_deckInfo.Price);
            }
        }
        else
        {
            _hideLock();

        }
    }
    
    private void _hideLock()
    {
        _lockGameObject.SetActive(false);
        _unlockSlider.gameObject.SetActive(false);
        _unlockButton.SetActive(false);
    }
    void _unlockWithPurchase()
    {
        if (A.GameSetting.ScoreSavable._ChangeAmount(-_deckInfo.Price))
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
        BAHMANMessageBoxManager.Instance._ShowMessage(A.Tags.NotEnoughCoinTag);
    }
    public void _ButtomClicked()
    {
        if (_deckInfo.IsLocked)
        {
            BAHMANMessageBoxManager.Instance._ShowYesNoBox(A.Tags.IsLockedTag, A.Tags.BuyDeckTag.Replace("&&&", _deckInfo.DeckName).Replace("$$$", A.Tools.ThousandSeparator(_deckInfo.Price)), _unlockWithPurchase);

        }
        else
        {

            A.GameSettings.CurrentDeckPosition = _deckOrder;
            OnDeckSelected?.Invoke();
            SoundManager._Instance._PlaySound(GameSounds.DeckSelect);
            //AdManager._Instance.ShowInterstitialAd();
            //BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.GameScene);
        }
    }
    private void AdManager__OnAdFailed()
    {

        BAHMANMessageBoxManager.Instance?._ShowMessage(A.Tags.PurchaseFailedTag);
        //BAHMANMessageBoxManager._INSTANCE._ShowMessage(A.Tags.CheckInternetConnection);
    }
    private void AdManager__OnAdSuccess()
    {
        BAHMANMessageBoxManager.Instance?._ShowMessage(A.Tags.PurchaseSuccessTag);
        _deckInfo.IsLocked = false;
        _hideLock();

    }

}

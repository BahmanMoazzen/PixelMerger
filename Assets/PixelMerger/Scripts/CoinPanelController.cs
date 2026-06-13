using System;
using System.Collections;
using UnityEngine;

public class CoinPanelController : MonoBehaviour
{
    const string SCORE_TAG = "PX_Scores";
    [SerializeField] TMPro.TextMeshProUGUI _coinText;
    [SerializeField] TMPro.TextMeshProUGUI _changeText;
    [SerializeField] Color _increaseColor = Color.green;
    [SerializeField] Color _decreaseColor = Color.red;
    [Range(0f, 10f)]
    [SerializeField] float _hideDelay = 2f;

    private void Awake()
    {
        _coinText.text = A.Tools.ThousandSeparator(A.GameSetting.ScoreSavable._Stock);
        _changeText.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        SaveableItem.OnValueChanged += OnScoreChanged;
    }
    private void OnDisable()
    {
        SaveableItem.OnValueChanged -= OnScoreChanged;
    }
    private void OnScoreChanged(SaveableItem iSaveable, int iAmountChanged)
    {
        if (iSaveable._Tag == SCORE_TAG)
        {
            _coinText.text = A.Tools.ThousandSeparator(A.GameSetting.ScoreSavable._Stock);
            _changeText.text = iAmountChanged.ToString();

            if (iAmountChanged < 0)
            {
                BAHMANSoundManager.Instance._PlaySound(GameSounds.CoinDecrease);
                _changeText.text = iAmountChanged.ToString();
                _changeText.color = _decreaseColor;


            }
            else
            {
                BAHMANSoundManager.Instance._PlaySound(GameSounds.CoinIncrease);
                _changeText.text = A.Tags.PLUS_SIGN + iAmountChanged.ToString();
                _changeText.color = _increaseColor;
            }
            _changeText.gameObject.SetActive(true);
            StartCoroutine(_hideChangeText());
        }

    }
    IEnumerator _hideChangeText()
    {
        yield return new WaitForSeconds(_hideDelay);
        _changeText.gameObject.SetActive(false);
    }

}

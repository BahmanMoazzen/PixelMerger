using System;
using UnityEngine;

public class CoinPanelController : MonoBehaviour
{
    const string SCORE_TAG = "PX_Scores";
    TMPro.TextMeshProUGUI _coinText;
    private void Awake()
    {
        _coinText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        _coinText.text = A.Tools.ThousandSeparator(A.GameSetting.ScoreSavable._Stock);
        
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
        }
    }
    
}

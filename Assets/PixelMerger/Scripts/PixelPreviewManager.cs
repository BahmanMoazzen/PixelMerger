using System;
using UnityEngine;
using UnityEngine.UI;

public class PixelPreviewManager : MonoBehaviour
{
    [SerializeField] GameObject[] _pixelPreviews;
    private void Start()
    {
        for (int i = 0; i < _pixelPreviews.Length; i++)
        {
            _pixelPreviews[i].GetComponent<Image>().sprite = A.GameSetting.AllDecks[A.GameSettings.CurrentDeckPosition].DeckPixels[i].MergerSprite;
        }
    }
    public void _ShowPixelPreview(int index)
    {
        
    }
}

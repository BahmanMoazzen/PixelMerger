using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PixelPreviewManager : MonoBehaviour
{
    [SerializeField] GameObject[] _pixelPreviews;
    public void Awake()
    {
        DeckButtomController.OnDeckSelected += _deckSelected;
    }
    public void OnDestroy()
    {
        DeckButtomController.OnDeckSelected -= _deckSelected;
    }
    void _deckSelected()
    {
        
        StartCoroutine(_createPreview());
        //BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.GameScene);
    }

    private void Start()
    {

        StartCoroutine(_createPreview());
    }

    IEnumerator _createPreview()
    {
        yield return null;
        for (int i = 0; i < _pixelPreviews.Length; i++)
        {
            _pixelPreviews[i].GetComponent<Image>().sprite = A.GameSetting.AllDecks[A.GameSettings.CurrentDeckPosition].DeckPixels[i].MergerSprite;
        }
    }
    public void _ShowPixelPreview(int iIndex)
    {
        BAHMANMessageBoxManager.Instance._ShowMessage(A.GameSetting.AllDecks[A.GameSettings.CurrentDeckPosition].DeckPixels[iIndex].MergerName, Color.white, 1.5f, A.GameSetting.AllDecks[A.GameSettings.CurrentDeckPosition].DeckPixels[iIndex].MergerSprite);
    }
}

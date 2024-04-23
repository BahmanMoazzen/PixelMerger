using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PublicRelationType { Share, Rate, OtherProduct, Donate ,Message}
[Serializable]
public struct PublicRelationSetting
{
    public string SceneName;
    public PublicRelationType RelationType;
    public int SceneCount;

}
[Serializable]
public struct PublicRelationMessageBoxInfo
{
    public string Title;
    public string Message;
}

public class BAHMANPublicRelation : MonoBehaviour
{
    const string PRPREFIX = "BPR";

    //[Header("Global Settings")]
    //[SerializeField] Text _messageBoxTitle;
    //[SerializeField] Text _messageBoxDesc;
    //[SerializeField] Button _messageBoxYes;
    //[SerializeField] GameObject _messageBox;
    [SerializeField] PublicRelationMessageBoxInfo[] _PRTitle;
    [SerializeField] PublicRelationSetting[] _AllSettings;
    [Header("Share Settings")]
    [Tooltip("sets the subject (primarily used in e-mail applications)")]
    [SerializeField] string _subject;
    [Tooltip("sets the shared text. Note that the Facebook app will omit text, if exists")]
    [SerializeField] string _text;
    [Tooltip("sets the title of the share dialog on Android platform.Has no effect on iOS")]
    [SerializeField] string _title;

    [Header("Rating Settings")]
    [SerializeField]
    string _ratingURL;
    [Header("Other Products Settings")]
    [SerializeField]
    string _otherProductURL;
    [Header("Donate Settings")]
    [SerializeField]
    string _donateURL;


    private void Awake()
    {

        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    private void SceneManager_sceneLoaded(Scene iLoadedScene, LoadSceneMode iLoadMode)
    {


        StopAllCoroutines();
        StartCoroutine(_scenLoadRoutine(iLoadedScene.name));

    }
    IEnumerator _scenLoadRoutine(string iSceneName)
    {
        string saveName = $"{PRPREFIX}_{iSceneName}_count";
        int newCount = PlayerPrefs.GetInt(saveName, 0) + 1;
        PlayerPrefs.SetInt(saveName, newCount);
        foreach (var prs in _AllSettings)
        {
            if (prs.SceneName.Equals(iSceneName))
            {
                if (newCount == prs.SceneCount)
                {
                    
                    switch (prs.RelationType)
                    {
                        case PublicRelationType.Share:
                            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(_PRTitle[(int)prs.RelationType].Title, _PRTitle[(int)prs.RelationType].Message, _ShareClicked);
                            
                            break;
                        case PublicRelationType.Rate:
                            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(_PRTitle[(int)prs.RelationType].Title, _PRTitle[(int)prs.RelationType].Message, _RateClicked);
                            
                            break;
                        case PublicRelationType.OtherProduct:
                            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(_PRTitle[(int)prs.RelationType].Title, _PRTitle[(int)prs.RelationType].Message, _OtherProductClicked);
                            
                            break;
                        case PublicRelationType.Donate:
                            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(_PRTitle[(int)prs.RelationType].Title, _PRTitle[(int)prs.RelationType].Message, _DonateClicked);
                            break;
                        case PublicRelationType.Message:
                            BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox(_PRTitle[(int)prs.RelationType].Title, _PRTitle[(int)prs.RelationType].Message, _MessageClicked);
                            break;
                    }
                    break;
                }

            }
            yield return null;
        }
    }
    public void _ShareClicked()
    {
        NativeShare NS = new NativeShare().SetSubject(_subject).SetText(string.Format(_text, Application.identifier)).SetTitle(_title);
        NS.Share();
        //_ClosePanel();
        
    }
    public void _OtherProductClicked()
    {
        Application.OpenURL(string.Format(_otherProductURL, Application.identifier));
        //_ClosePanel();

    }
    public void _RateClicked()
    {
        Application.OpenURL(string.Format( _ratingURL,Application.identifier));
        //_ClosePanel();
    }
    public void _DonateClicked()
    {
        Application.OpenURL(_donateURL);
        //_ClosePanel();
    }
    public void _MessageClicked()
    {
        //Application.OpenURL(_donateURL);
        //_ClosePanel();
    }
   



}

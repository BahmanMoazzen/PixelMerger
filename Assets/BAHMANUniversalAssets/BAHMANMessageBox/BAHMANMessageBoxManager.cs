/*
 * Message box Version 1.2
 * This module shows timed messages on the screen
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Message structure to show on screen
/// </summary>
class MessageStruct
{
    public Color _color;
    public string _message;
    public float _interval;

}
public class BAHMANMessageBoxManager : MonoBehaviour
{
    #region public methods 
    /// <summary>
    /// instance to call message box manager
    /// </summary>
    public static BAHMANMessageBoxManager _INSTANCE;

    public bool IsReady
    {
        get
        {
            return _messageQueue != null;
        }
    }
    /// <summary>
    /// insert message by single text
    /// </summary>
    /// <param name="iMessage">the message to show</param>
    public void _ShowMessage(string iMessage)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = _DefaultColor;
        messageStructure._interval = _DefaultHideIntervalTime;
        messageStructure._message = BAHMANLanguageManager._Instance._Translate(iMessage);
        //if (_messageQueue == null)
        //{
        //    _messageQueue = new Queue<MessageStruct>();
        //}
        _messageQueue? .Enqueue(messageStructure);


    }
    /// <summary>
    /// insert message by single text
    /// </summary>
    /// <param name="iMessage">the message to show</param>
    /// <param name="iInterval">time to display on screen</param>
    public void _ShowMessage(string iMessage, float iInterval)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = _DefaultColor;
        messageStructure._interval = iInterval;
        messageStructure._message = BAHMANLanguageManager._Instance._Translate(iMessage);
        //if(_messageQueue == null)
        //{
        //    _messageQueue = new Queue<MessageStruct> ();
        //}
        _messageQueue.Enqueue(messageStructure);


    }
    /// <summary>
    /// insert message by single text and color
    /// </summary>
    /// <param name="iMessage">the message to display</param>
    /// <param name="iColor">the color of the text</param>
    public void _ShowMessage(string iMessage, Color iColor)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = iColor;
        messageStructure._message = BAHMANLanguageManager._Instance._Translate(iMessage);
        messageStructure._interval = _DefaultHideIntervalTime;
        //if (_messageQueue == null)
        //{
        //    _messageQueue = new Queue<MessageStruct>();
        //}
        _messageQueue.Enqueue(messageStructure);


    }
    /// <summary>
    /// insert message by single text and color and time
    /// </summary>
    /// <param name="iMessage">the message to show</param>
    /// <param name="iColor">the color of the text</param>
    /// <param name="iInterval">time to display on screen</param>
    public void _ShowMessage(string iMessage, Color iColor, float iInterval)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = iColor;
        messageStructure._message = BAHMANLanguageManager._Instance._Translate(iMessage);
        messageStructure._interval = iInterval;
        //if (_messageQueue == null)
        //{
        //    _messageQueue = new Queue<MessageStruct>();
        //}
        _messageQueue.Enqueue(messageStructure);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="iTitle"></param>
    /// <param name="iMessage"></param>
    /// <param name="iYesButtonText"></param>
    /// <param name="iNoButtonText"></param>
    /// <param name="iShowCloseButton"></param>
    /// <param name="iShowTitleBar"></param>
    /// <param name="iCloseAction"></param>
    /// <param name="iYesAction"></param>
    /// <param name="iNoAction"></param>


    public void _ShowYesNoBox(string iTitle, string iMessage, string iYesButtonText, string iNoButtonText, bool iShowCloseButton, bool iShowTitleBar, UnityAction iCloseAction, UnityAction iYesAction, UnityAction iNoAction)
    {
        _yesNoController._ShowPanel(BAHMANLanguageManager._Instance._Translate(iTitle), BAHMANLanguageManager._Instance._Translate(iMessage)
            , BAHMANLanguageManager._Instance._Translate(iYesButtonText), BAHMANLanguageManager._Instance._Translate(iNoButtonText)
            , iShowCloseButton, iShowTitleBar, iCloseAction, iYesAction, iNoAction); ;
    }

    public void _ShowYesNoBox(string iTitle, string iMessage, UnityAction iYesAction)
    {
        _yesNoController._ShowPanel(BAHMANLanguageManager._Instance._Translate(iTitle), BAHMANLanguageManager._Instance._Translate(iMessage)
            , BAHMANLanguageManager._Instance._Translate(YESTAG)
            , BAHMANLanguageManager._Instance._Translate(NOTAG)
            , true, true, null, iYesAction, null);
    }
    public void _ShowConfirmBox(string iTitle, string iMessage, string iConfirmButtonText, bool iShowCloseButton, bool iShowTitleBar, UnityAction iCloseAction, UnityAction iConfirmAction)
    {
        _confirmController._ShowPanel(BAHMANLanguageManager._Instance._Translate(iTitle), BAHMANLanguageManager._Instance._Translate(iMessage)
            , BAHMANLanguageManager._Instance._Translate(iConfirmButtonText),
            iShowCloseButton, iShowTitleBar, iCloseAction, iConfirmAction);
    }
    public void _ShowConfirmBox(string iTitle, string iMessage)
    {
        _confirmController._ShowPanel(BAHMANLanguageManager._Instance._Translate(iTitle), BAHMANLanguageManager._Instance._Translate(iMessage),
            BAHMANLanguageManager._Instance._Translate(CONFIRMTAG), true, true, null, null);
    }
    public void _ShowConfirmBox(string iMessage)
    {
        _confirmController._ShowPanel(null, BAHMANLanguageManager._Instance._Translate(iMessage),
            BAHMANLanguageManager._Instance._Translate(CONFIRMTAG), false, false, null, null);
    }
    #endregion
    #region private methods


    /// <summary>
    /// message text placeholder
    /// </summary>
    [SerializeField] Text _MessageText;

    /// <summary>
    /// message panel to show or hide
    /// </summary>
    [SerializeField] GameObject _MessagePanel;

    /// <summary>
    /// default hide interval time
    /// </summary>
    [SerializeField][Range(0, 10)] float _DefaultHideIntervalTime = 2f;


    /// <summary>
    /// default hide interval time
    /// </summary>
    [SerializeField] Color _DefaultColor = Color.white;


    /// <summary>
    /// message queue for storing data
    /// </summary>
    Queue<MessageStruct> _messageQueue;


    YesNoPannelController _yesNoController;

    ConfirmPanelController _confirmController;

    const string YESTAG = "Yes", NOTAG = "No", CONFIRMTAG = "Confirm";




    void Awake()
    {

        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            _confirmController = GetComponent<ConfirmPanelController>();
            _yesNoController = GetComponent<YesNoPannelController>();
            StartCoroutine(_startupRoutine());
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    


    IEnumerator _startupRoutine()
    {
        yield return 0;
        _messageQueue = new Queue<MessageStruct>();
        _MessagePanel.SetActive(false);
        _MessageText.text = string.Empty;
        StartCoroutine(_MessageManager());
        yield return 0;
    }
    IEnumerator _MessageManager()
    {
        while (true)
        {
            if (_messageQueue.Count > 0)
            {
                var message = _messageQueue.Dequeue();
                if (message._interval < 0)
                {
                    if (_messageQueue.Count <= 0)
                    {
                        _MessagePanel.SetActive(true);
                        _MessageText.text = message._message;
                        _MessageText.color = message._color;
                    }

                }
                else
                {
                    _MessagePanel.SetActive(true);
                    _MessageText.text = message._message;
                    _MessageText.color = message._color;
                    yield return new WaitForSeconds(message._interval);
                    _MessagePanel.SetActive(false);
                    _MessageText.text = string.Empty;
                }
            }
            yield return 0;
        }

    }
    #endregion

}



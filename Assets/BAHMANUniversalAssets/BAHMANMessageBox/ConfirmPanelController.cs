using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmPanelController : MonoBehaviour
{
    [SerializeField] Text _titleText;
    [SerializeField] Text _messageText;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _confirmButton;
    [SerializeField] Text _confirmButtonText;
    [SerializeField] GameObject _confirmPanel;
    [SerializeField] GameObject _titlePanel;
    /// <summary>
    /// shows a confirm panel
    /// </summary>
    /// <param name="iTitle">the title to show in Confirm panel</param>
    /// <param name="iMessage">the message in Confirm panel</param>
    /// <param name="iConfirmButtonText">the confirm button caption in Confirm panel</param>
    /// <param name="iShowCloseButton">whether show close button in Confirm Panel</param>
    /// <param name="iShowTitleBar">whether show title bar in Confirm Panel</param>
    /// <param name="iCloseAction">the action to run on close button clicks</param>
    /// <param name="iConfirmAction">the action to run when confirm button clicks</param>
    public void _ShowPanel(string iTitle, string iMessage, string iConfirmButtonText, bool iShowCloseButton, bool iShowTitleBar, UnityAction iCloseAction, UnityAction iConfirmAction)
    {

        _titleText.text = iTitle;


        _messageText.text = iMessage;
        _confirmButtonText.text = iConfirmButtonText;

        _closeButton.gameObject.SetActive(iShowCloseButton);
        _titlePanel.SetActive(iShowTitleBar);

        _confirmButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();


        _closeButton.onClick.AddListener(_closePanel);
        _confirmButton.onClick.AddListener(_closePanel);

        if (iConfirmAction != null)
        {
            _confirmButton.onClick.AddListener(iConfirmAction);
        }
        if (iCloseAction != null)
        {
            _confirmButton.onClick.AddListener(iCloseAction);
        }
        _confirmPanel.SetActive(true);
    }
    /// <summary>
    /// closes the confirm panel
    /// </summary>
    void _closePanel()
    {
        _confirmPanel.SetActive(false);
    }
}

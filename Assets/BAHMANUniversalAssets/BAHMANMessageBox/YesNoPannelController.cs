using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class YesNoPannelController : MonoBehaviour
{

    [SerializeField] Text _titleText;
    [SerializeField] Text _messageText;
    [SerializeField] Button _closeButton;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;
    [SerializeField] Text _yesButtonText;
    [SerializeField] Text _noButtonText;
    [SerializeField] GameObject _yesNoPanel;
    [SerializeField] GameObject _titlePanel;
    /// <summary>
    /// Shows a Yes/No panel and runs the proper action on yeach button
    /// </summary>
    /// <param name="iTitle">the title to show in Yes/No panel</param>
    /// <param name="iMessage">the message of Yes/No panel</param>
    /// <param name="iYesButtonText">the caption of Yes button in Yes/No panel</param>
    /// <param name="iNoButtonText">the caption of No button in Yes/No panel</param>
    /// <param name="iShowCloseButton">whether show close button in Yes/No panel</param>
    /// <param name="iShowTitleBar">whether show the title bar or not</param>
    /// <param name="iCloseAction">the action that runs after clicking cloase button</param>
    /// <param name="iYesAction">the action that runs after clickong yes button</param>
    /// <param name="iNoAction">the action that runs after clicking no button</param>
    public void _ShowPanel(string iTitle, string iMessage, string iYesButtonText, string iNoButtonText, bool iShowCloseButton, bool iShowTitleBar, UnityAction iCloseAction, UnityAction iYesAction, UnityAction iNoAction)
    {

        _titleText.text = iTitle;


        _messageText.text = iMessage;
        _yesButtonText.text = iYesButtonText;
        _noButtonText.text = iNoButtonText;



        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();


        _closeButton.onClick.AddListener(_closePanel);
        _yesButton.onClick.AddListener(_closePanel);
        _noButton.onClick.AddListener(_closePanel);

        _closeButton.gameObject.SetActive(iShowCloseButton);
        _titlePanel.SetActive(iShowTitleBar);

        if (iYesAction != null)
        {
            _yesButton.onClick.AddListener(iYesAction);
        }
        if (iNoAction != null)
        {
            _noButton.onClick.AddListener(iNoAction);
        }
        if (iCloseAction != null)
        {
            _closeButton.onClick.AddListener(iCloseAction);
        }
        _yesNoPanel.SetActive(true);
    }
    /// <summary>
    /// closes the Yes/No panel
    /// </summary>
    void _closePanel()
    {
        _yesNoPanel.SetActive(false);
    }
}

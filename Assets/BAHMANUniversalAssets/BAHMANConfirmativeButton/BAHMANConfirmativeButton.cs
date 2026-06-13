using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class BAHMANConfirmativeButton : MonoBehaviour
{
    [Header("Confirmation Message")]

    [SerializeField] string messageBoxTitle = "Are you sure?";
    [SerializeField] string messageBoxMessage = "Do you really want to do this?";
    [SerializeField] string messageBoxYes = "Yes";
    [SerializeField] string messageBoxNo = "No";

    [Header("Events")]
    public UnityEvent OnWindowClosed;
    public UnityEvent OnConfirmEvents;
    public UnityEvent OnDenyEvents;

    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(showConfirmationMessage);
    }
    void showConfirmationMessage()
    {
        //OnClicked?.Invoke();
        BAHMANMessageBoxManager.Instance._ShowYesNoBox(messageBoxTitle, messageBoxMessage, messageBoxYes, messageBoxNo, true, true, windowClosed, confirmed, denied);
    }

    void confirmed()
    {
        OnConfirmEvents?.Invoke();
    }
    void denied()
    {
        OnDenyEvents?.Invoke();

    }
    void windowClosed()
    {
        OnWindowClosed?.Invoke();
    }
}
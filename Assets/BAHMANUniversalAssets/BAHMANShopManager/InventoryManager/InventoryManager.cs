/*
 * 
 *  Inventory System Version 1.0
 * 
 * 
 */


using System.Collections;
using UnityEngine;
using UnityEngine.UI;



// inventory data structure
[System.Serializable]
public struct InventoryItem
{
    public Image _PlaceHolderImage;
    public Text _AmountPlaceholderText;
    public Text _ChangeAmountPlaceholderText;
    public SaveableItem _Item;
}
public class InventoryManager : MonoBehaviour
{

    // instance to handle with inventory
    public static InventoryManager _INSTANCE;

    // this class has no public procedure. use saveable object to change item amount

    #region private

    // the panel to show or hide inventory
    [SerializeField] GameObject _InventoryPanel;

    // hide after inventory showed
    public bool _AutoHide = false;

    // automaticly refresh inventory ui
    [SerializeField] bool _AutoRefresh = false;

    [SerializeField] bool _universalObject = false;

    // ui show interval
    [SerializeField][Range(0, 10)] float _ShowHoldTime = 1.5f;

    // all saveable inventory items
    [SerializeField] InventoryItem[] _Items;



    void Awake()
    {
        if (_INSTANCE == null)
            _INSTANCE = this;
        else
            Destroy(gameObject);
        if (_universalObject)
            DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        StartCoroutine(_startupRoutine());
    }
    IEnumerator _startupRoutine()
    {

        if (_AutoHide)
        {
            _InventoryPanel.SetActive(false);
        }
        _UpdateVisual();
        yield return 0;
    }

    
    void OnEnable()
    {
        SaveableItem.OnValueChanged += _SaveableChanged;
    }
    void OnDisable()
    {
        SaveableItem.OnValueChanged -= _SaveableChanged;
    }
    public void _ShowPanel(bool iUpdateBeforShow)
    {
        StopAllCoroutines();
        if (iUpdateBeforShow)
            _UpdateVisual();
        _InventoryPanel.SetActive(true);
        if (_AutoHide)
            StartCoroutine(_closePanel());
    }
    void _SaveableChanged(SaveableItem iItem, int iAmount)
    {

        _ShowPanel(false);
        StartCoroutine(_saveableChangedRoutine(iItem, iAmount));

    }
    IEnumerator _saveableChangedRoutine(SaveableItem iItem, int iAmount)
    {
        foreach (var i in _Items)
        {
            if (i._Item._Tag.Equals(iItem._Tag))
            {
                i._ChangeAmountPlaceholderText.gameObject.SetActive(true);
                i._ChangeAmountPlaceholderText.text = Mathf.Abs(iAmount).ToString();
                if (iAmount > 0)
                {
                    i._ChangeAmountPlaceholderText.text += "+";
                    i._ChangeAmountPlaceholderText.color = Color.green;
                }
                else
                {
                    i._ChangeAmountPlaceholderText.text += "-";
                    i._ChangeAmountPlaceholderText.color = Color.red;
                }
                yield return new WaitForSeconds(_ShowHoldTime / 2);
                i._ChangeAmountPlaceholderText.gameObject.SetActive(false);
                _UpdateVisual();

            }
        }
    }
    IEnumerator _closePanel()
    {
        yield return new WaitForSeconds(_ShowHoldTime);
        _InventoryPanel.SetActive(false);
    }
    public void _UpdateVisual()
    {
        foreach (var item in _Items)
        {
            item._PlaceHolderImage.sprite = item._Item._Icon;
            item._AmountPlaceholderText.text = item._Item._Stock.ToString();
        }
    }
    void FixedUpdate()
    {
        if (_AutoRefresh)
        {
            _UpdateVisual();
        }
    }

    #endregion

}

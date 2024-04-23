/*
 BAHMAN Back Button Handler V.1.1
this module handles back button on mobile devices and Escape button on desktops
It should be loaded in the very first scene because it won't be destroyed on load.
use the prefab provided in this folder to modify the look of the message box.
*/

using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class BAHMANBackButtonManager : MonoBehaviour
{
    //events to fire 
    public static event UnityAction OnBackButtonMenuShowed;
    public static event UnityAction OnBackButtonMenuHide;

    const string TITLETAG = "", MESSAGETAG = "Do You Want To Quit Game Or Go To Home Screen?", EXITTAG="Exit", HOMETAG = "Home";

    public static BAHMANBackButtonManager _Instance;

    [Tooltip("check this if you need just trigger the events")]
    [SerializeField] bool _SilentMode = false;

    [Tooltip("the scene name of the home button")]
    [SerializeField] AllScenes _HomeSceneName;

    [Tooltip("the default panel to show when back button pressed")]

    //the menu item to insert back button prefab
    const string _prefabName = "BAHMANBackButtonManager";
    bool _isBackPanelActive = false;

    //[MenuItem("BAHMAN Unity Assets/Create Back Button Manager", false, 2)]
    //static void CreateCustomGameObject(MenuCommand menuCommand)
    //{

    //    GameObject newGo = Instantiate(Resources.Load<GameObject>(_prefabName), Vector3.zero, Quaternion.identity);

    //    //GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
    //    //Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
    //    newGo.name = _prefabName;
    //    Selection.activeObject = newGo;

    //}


    void Awake()
    {

        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isBackPanelActive)
            
            {
                _isBackPanelActive = true;
                _ShowMenu();
                OnBackButtonMenuShowed?.Invoke();
            }
        }
    }
    public void _ShowMenu()
    {
        _isBackPanelActive = true;
        BAHMANMessageBoxManager._INSTANCE._ShowYesNoBox("End Game","Do You Want To Quit Game Or Go To Home Screen?","Exit","Home",true,true,_closeClicked,_Exit,_Home);
        OnBackButtonMenuShowed?.Invoke();
    }
    public void _Exit()
    {
        Application.Quit();
    }
    public void _Home()
    {
        _isBackPanelActive = false;
        BAHMANLoadingManager._INSTANCE._LoadScene(_HomeSceneName);

        //LoadingManager._INSTANCE._LoadScene(_HomeSceneName);
    }
    void _closeClicked()
    {
        _isBackPanelActive = false;
        OnBackButtonMenuHide?.Invoke();
    }

}


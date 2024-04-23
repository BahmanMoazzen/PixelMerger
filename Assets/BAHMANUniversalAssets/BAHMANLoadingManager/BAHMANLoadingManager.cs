/*
 * 
 * Loading Manager Version 1.1
 * 
 */


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// all the scens in the build list of the game
/// </summary>
public enum AllScenes { LoadingScene, TitleScreenScene, GameScene, AftermathScene }


public class BAHMANLoadingManager : MonoBehaviour
{
    /// <summary>
    /// instance to call Load manager
    /// </summary>
    public static BAHMANLoadingManager _INSTANCE;

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="iSceneIndex">the scene index in build list</param>
    public void _LoadScene(int iSceneIndex)
    {
        StartCoroutine(_loadSceneRoutin(iSceneIndex));

    }
    /// <summary>
    /// load a scene
    /// </summary>
    /// <param name="iSceneName">the scene enum</param>
    public void _LoadScene(AllScenes iSceneName)
    {
        StartCoroutine(_loadSceneRoutin((int)iSceneName));

    }
    /// <summary>
    /// load a scene
    /// </summary>
    /// <param name="iSceneName">the name of a scene in build list</param>
    public void _LoadScene(string iSceneName)
    {

        StartCoroutine(_loadSceneRoutin(iSceneName));

    }
    /// <summary>
    /// manually hides load panel
    /// </summary>
    public void _LoadCompeleted()
    {
        _HideLoadPanel();
    }

    /// <summary>
    /// Manually shows load panel
    /// </summary>
    public void _StartLoading()
    {
        _ShowLoadPanel();
    }

    #region private
    /// <summary>
    /// the loading slider
    /// </summary>
    [SerializeField] GameObject _LoadingSlider;

    /// <summary>
    /// panel to show or hide entire loading 
    /// </summary>
    [SerializeField] GameObject _LoadPanel;

    /// <summary>
    /// show on start of scene
    /// </summary>
    [SerializeField] bool _ShowOnStartup = true;

    /// <summary>
    /// autohide loading panel after load
    /// </summary>
    [SerializeField] bool _AutoHideOnLoad = true;



    /// <summary>
    /// setting SingleTone parameters
    /// </summary>
    void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    /// <summary>
    /// registering event listeners
    /// </summary>
    void OnEnable()
    {

        SceneManager.sceneLoaded += _sceneLoadedComplete;


    }
    /// <summary>
    /// unregistering event listeners
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= _sceneLoadedComplete;


    }
    /// <summary>
    /// fires when ever a new scen loads
    /// </summary>
    /// <param name="iScene">the scene to load</param>
    /// <param name="iMode">defines how to load the scene</param>
    void _sceneLoadedComplete(Scene iScene, LoadSceneMode iMode)
    {
        _LoadPanel.SetActive(_ShowOnStartup);
        if (_AutoHideOnLoad)
        {
            _HideLoadPanel();
        }
    }
    /// <summary>
    /// triggers whenever new scene loaded
    /// </summary>
    /// <param name="iSceneIndex">the scene index in build scene</param>
    /// <returns></returns>
    IEnumerator _loadSceneRoutin(int iSceneIndex)
    {
        _ShowLoadPanel();
        yield return 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(iSceneIndex, LoadSceneMode.Single);
        _LoadingSlider.SetActive(true);
        while (!asyncLoad.isDone)
        {
            _LoadingSlider.GetComponent<Slider>().value = asyncLoad.progress;
            yield return null;

        }
        _LoadingSlider.SetActive(false);

    }
    /// <summary>
    /// triggers whenever new scene loaded
    /// </summary>
    /// <param name="iSceneName">the name of the scene in build index</param>
    /// <returns></returns>
    IEnumerator _loadSceneRoutin(string iSceneName)
    {

        _ShowLoadPanel();
        yield return 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(iSceneName, LoadSceneMode.Single);
        _LoadingSlider.SetActive(true);
        while (!asyncLoad.isDone)
        {
            _LoadingSlider.GetComponent<Slider>().value = asyncLoad.progress;
            yield return null;
        }
        _LoadingSlider.SetActive(false);

    }
    /// <summary>
    /// shows load panel
    /// </summary>
    void _ShowLoadPanel()
    {
        _LoadPanel.SetActive(true);
    }
    /// <summary>
    /// hides load panel
    /// </summary>
    void _HideLoadPanel()
    {
        _LoadPanel.SetActive(false);
    }

    #endregion


}

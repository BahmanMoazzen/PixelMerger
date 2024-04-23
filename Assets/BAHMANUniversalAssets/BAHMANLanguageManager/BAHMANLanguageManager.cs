/*
 *  Language Translator Ver. 1.0
 * 
 * 
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BAHMANLanguageManager : MonoBehaviour
{
    /// <summary>
    /// the instance to reference the class
    /// </summary>
    public static BAHMANLanguageManager _Instance;
    /// <summary>
    /// the language setting of the game
    /// </summary>
    [Tooltip("Language Words Settings")]
    [SerializeField] LanguageManagerInfo _languageManagerInfo;
    /// <summary>
    /// current Scene translation routine
    /// </summary>
    Coroutine _tRoutine;
    private void Awake()
    {
        if (_Instance == null) _Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        StartCoroutine(_translateCoroutine(gameObject.scene));


    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;


    }
    private void OnDisable()
    {

        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

    }
    private void SceneManager_sceneLoaded(Scene iCurrentScene, LoadSceneMode arg1)
    {
        // cancel translation of previous scene if new scene is loaded
        if (_tRoutine != null) StopCoroutine(_tRoutine);
        // translating active scene
        _tRoutine = StartCoroutine(_translateCoroutine(iCurrentScene));

    }
    /// <summary>
    /// Translates all the elements in current scene and dontdestroyscene
    /// </summary>
    public void _TranslateActiveScenes()
    {
        StopAllCoroutines();
        StartCoroutine(_translateCoroutine(gameObject.scene));
        StartCoroutine(_translateCoroutine(SceneManager.GetActiveScene()));

    }
    /// <summary>
    /// the scene translation routine
    /// </summary>
    /// <param name="iScene">the scene to translate</param>
    /// <returns></returns>
    IEnumerator _translateCoroutine(Scene iScene)
    {
        yield return null;

        if (_languageManagerInfo.IsActive)
        {
            foreach (GameObject element in iScene.GetRootGameObjects())
            {

                foreach (Text txt in element.GetComponentsInChildren<Text>(true))
                {
                    txt.text = _languageManagerInfo.TranslateWord(txt.text);

                }
                foreach (TextMeshPro txt in element.GetComponentsInChildren<TextMeshPro>(true))
                {
                    txt.text = _languageManagerInfo.TranslateWord(txt.text);
                }
                foreach (TextMeshProUGUI txt in element.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    txt.text = _languageManagerInfo.TranslateWord(txt.text);
                }

            }
        }

    }

    /// <summary>
    /// translates the word into active language
    /// </summary>
    /// <param name="iWord">the word to translate</param>
    /// <returns></returns>
    public string _Translate(string iWord)
    {
        return _languageManagerInfo.IsActive ? _languageManagerInfo.TranslateWord(iWord) : iWord;
    }



}

/*
 BAHMAN Music Box V.2.1
This piece of code detects the loaded scene and plays the proper clip for it.
It should be loaded in the very first scene because it won't be destroyed on load.

*/


using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MusicPlayer))]
public class BAHMANMusicBox : MonoBehaviour
{
    public static BAHMANMusicBox _Instance;

    private static byte _InstanceCount = 0;

    //constant values of the music box
    public static int MIN_VOLUME = 0, INVALID_INDEX_NUMBER = -1;

    [Header("Music Box Global Settings")]

    //if scene is not defined countinue previous music or stop that.
    [Tooltip("How sould handle undefined scenes?")]
    [SerializeField] bool _StopPlayingOnUndefinedScenes;

    // soart out all musics in game
    [Header("Each Scene Settings")]
    [SerializeField] MusicBoxSceneInfo[] _musicBoxSceneInfos;

    // the class uses audio source to play music
    MusicPlayer _musicPlayerClass;

    //the menu item to easily insert the music box
    //[MenuItem("BAHMAN Unity Assets/Create Music Box", false, 4)]
    //static void CreateCustomGameObject(MenuCommand menuCommand)
    //{
    //    GameObject go = new GameObject("BAHMAN Music Box");
    //    go.AddComponent<BAHMANMusicBox>();
    //    GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
    //    Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
    //    Selection.activeObject = go;
    //}


    //registering for scene change handler.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        GameSettingInfo.OnMusicChange += _ChangePlayingStat;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        GameSettingInfo.OnMusicChange -= _ChangePlayingStat;
    }
    private void SceneManager_sceneLoaded(Scene iLoadedScene, LoadSceneMode iLoadMethod)
    {
        StopAllCoroutines();
        StartCoroutine(_sceneLoaded(iLoadedScene.buildIndex));

    }
    IEnumerator _sceneLoaded(int iSceneBuildIndex)
    {
        //find current scene setting
        int sceneMusicIndex = INVALID_INDEX_NUMBER;
        for (byte i = 0; i < _musicBoxSceneInfos.Length; i++)
        {
            if ((int)_musicBoxSceneInfos[i]._SceneName == iSceneBuildIndex)
            {
                if (_musicBoxSceneInfos[i]._SceneMusics.Length > 0)
                    sceneMusicIndex = i;
                break;
            }
        }
        if (sceneMusicIndex > INVALID_INDEX_NUMBER)
        {
            //scene info is available
            if (_musicPlayerClass == null)
            {
                _musicPlayerClass = GetComponent<MusicPlayer>();

            }
            StartCoroutine(_musicPlayerClass._SceneChanged(_musicBoxSceneInfos[sceneMusicIndex]));
        }
        else
        {
            //scene info is not available
            if (_StopPlayingOnUndefinedScenes)
            {
                _musicPlayerClass?._StopPlaying();
            }
        }
        yield return null;
    }
    
    public void _ChangePlayingStat([Tooltip("True = Start Play; False=Stop Play")] bool iPlayingStat)
    {
        if (iPlayingStat)
        {
            StartCoroutine(_sceneLoaded(SceneManager.GetActiveScene().buildIndex));
        }
        else
        {
            StopAllCoroutines();
            _musicPlayerClass?._StopPlaying();
        }
    }

    private void Awake()
    {
        if (_InstanceCount > 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _InstanceCount++;
            DontDestroyOnLoad(this);
            _Instance = this;
        }
    }


}


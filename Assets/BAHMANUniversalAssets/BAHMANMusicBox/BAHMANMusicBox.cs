using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MusicPlayerController))]
public class BAHMANMusicBox : MonoBehaviour
{
    public static BAHMANMusicBox Instance { get; private set; }

    [Header("Music Box Global Settings")]
    [Tooltip("How should handle undefined scenes?")]
    [SerializeField]
    private bool _stopPlayingOnUndefinedScenes = true;

    [Header("Config Repository")]
    [SerializeField]
    private ScriptableObjectMusicBoxConfigRepository _configRepository;

    private MusicPlayerController _musicPlayer;
    private Coroutine _sceneLoadRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicPlayer = GetComponent<MusicPlayerController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameSettingInfo.OnMusicChange += OnMusicSettingChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameSettingInfo.OnMusicChange -= OnMusicSettingChanged;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneLoadRoutine != null)
        {
            StopCoroutine(_sceneLoadRoutine);
        }

        _sceneLoadRoutine = StartCoroutine(HandleSceneLoaded(scene.buildIndex));
    }

    private IEnumerator HandleSceneLoaded(int sceneBuildIndex)
    {
        yield return null; // allow one frame for scene init if needed

        if (!A.GameSetting.Music)
            yield break;

        if (_configRepository != null &&
            _configRepository.TryGetSceneConfig(sceneBuildIndex, out var config) &&
            config.HasClips)
        {
            _musicPlayer.PlayScene(config);
        }
        else
        {
            if (_stopPlayingOnUndefinedScenes)
            {
                _musicPlayer.StopAll();
            }
        }
    }

    private void OnMusicSettingChanged(bool isMusicOn)
    {
        if (isMusicOn)
        {
            _musicPlayer.SetMixerVolume(1f);
            // restart for current scene
            var activeScene = SceneManager.GetActiveScene();
            if (_sceneLoadRoutine != null)
                StopCoroutine(_sceneLoadRoutine);

            _sceneLoadRoutine = StartCoroutine(HandleSceneLoaded(activeScene.buildIndex));
        }
        else
        {
            if (_sceneLoadRoutine != null)
            {
                StopCoroutine(_sceneLoadRoutine);
                _sceneLoadRoutine = null;
            }
            _musicPlayer.SetMixerVolume(0f);
            _musicPlayer.StopAll();
        }
    }
}

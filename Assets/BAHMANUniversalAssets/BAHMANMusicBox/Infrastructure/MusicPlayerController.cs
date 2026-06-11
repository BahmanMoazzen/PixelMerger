using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayerController : MonoBehaviour
{
    public delegate void MusicStarted(string musicTitle);
    public static event MusicStarted OnMusicStarted;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _volumeParameter = "MusicVolume";
    [SerializeField]
    private float _messageInterval = 2.5f;

    private AudioSource _audioSource;
    private Coroutine _playRoutine;
    private MusicPlaylist _playlist;
    private MusicBoxSceneConfig _currentConfig;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void SetMixerVolume(float normalizedVolume)
    {
        // normalizedVolume = 0..1
        float dB = Mathf.Lerp(-80f, 0f, normalizedVolume);
        _audioMixer.SetFloat(_volumeParameter, dB);
    }
    public void StopAll()
    {
        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
            _playRoutine = null;
        }

        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    public void PlayScene(MusicBoxSceneConfig config)
    {
        _currentConfig = config;
        _playlist = new MusicPlaylist(config);

        if (_playRoutine != null)
        {
            StopCoroutine(_playRoutine);
        }

        _playRoutine = StartCoroutine(PlaySceneRoutine());
    }

    private IEnumerator PlaySceneRoutine()
    {
        while (true)
        {
            if (_audioSource.isPlaying)
            {
                if (_currentConfig.StopMusicGradually)
                {
                    yield return StartCoroutine(FadeOutMixer(_currentConfig.FadeInterval));
                }

                _audioSource.Stop();

                if (_currentConfig.SilenceBetweenClips > 0f)
                    yield return new WaitForSeconds(_currentConfig.SilenceBetweenClips);
            }

            var nextClip = _playlist.GetNextClip();
            if (nextClip == null)
            {
                // playlist finished
                _playRoutine = null;
                yield break;
            }

            _audioSource.clip = nextClip;
            SetMixerVolume(_currentConfig.GlobalVolume);

            //_audioSource.volume = _currentConfig.GlobalVolume;
            _audioSource.Play();

            if (BAHMANMessageBoxManager.Instance.IsReady)
            {
                BAHMANMessageBoxManager.Instance._ShowMessage(
                    _audioSource.clip.name,
                    _messageInterval);
            }

            OnMusicStarted?.Invoke(_audioSource.clip.name);

            float playTime = _audioSource.clip.length;
            if (_currentConfig.StopMusicGradually)
            {
                float fadeStartTime = Mathf.Max(0f, playTime - _currentConfig.FadeInterval);
                yield return new WaitForSeconds(fadeStartTime);
                yield return StartCoroutine(FadeOutMixer(_currentConfig.FadeInterval));
            }
            else
            {
                yield return new WaitForSeconds(playTime);
            }
        }
    }

    private IEnumerator FadeOutRoutine(float targetVolume, float duration)
    {
        float startVolume = _audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        _audioSource.volume = targetVolume;
    }
    private IEnumerator FadeOutMixer(float duration)
    {
        float startValue;
        _audioMixer.GetFloat(_volumeParameter, out startValue);

        float startLinear = Mathf.InverseLerp(-80f, 0f, startValue);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float newLinear = Mathf.Lerp(startLinear, 0f, 1f - t);
            float dB = Mathf.Lerp(-80f, 0f, newLinear);
            _audioMixer.SetFloat(_volumeParameter, dB);
            yield return null;
        }

        _audioMixer.SetFloat(_volumeParameter, -80f);
    }

}

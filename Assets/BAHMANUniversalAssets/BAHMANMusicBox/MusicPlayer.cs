using System.Collections;
using UnityEngine;


public class MusicPlayer : MonoBehaviour
{

    //event to invoke after music started to play returns the name of music
    public delegate void MusicStarted(string iMusicTitle);
    public static event MusicStarted OnMusicStarted;

    const float MESSAGEINTERVAL = 2.5f;


    //the audio source to play music in it
    AudioSource _musicPlayer;

    //the information about musics in scene
    MusicBoxSceneInfo _sceneMusicInfo;

    //the current index of music being played
    int _currentMusicIndex = BAHMANMusicBox.INVALID_INDEX_NUMBER;


    public void _StopPlaying()
    {
        _musicPlayer.Stop();
        StopAllCoroutines();
    }
    int _nextIndext(int iCurrentMusic)
    {
        int musicIndex = 0;
        if (_sceneMusicInfo._ShufflePlay)
        {
            musicIndex = Random.Range(0, _sceneMusicInfo._SceneMusics.Length);

            // this methos uses a lot of CPU power.
            //do
            //{
            //    musicIndex = Random.Range(0, _sceneMusicInfo._SceneMusics.Length);
            //} while (musicIndex == iCurrentMusic);
        }
        else
        {
            musicIndex = iCurrentMusic + 1;
            if (musicIndex >= _sceneMusicInfo._SceneMusics.Length)
            {
                if (_sceneMusicInfo._StopAfterListEnded) { musicIndex = BAHMANMusicBox.INVALID_INDEX_NUMBER; }
                else { musicIndex = 0; }
            }
        }
        return musicIndex;
    }
    public IEnumerator _SceneChanged(MusicBoxSceneInfo iSceneInfo)
    {
        StopAllCoroutines();
        if (_musicPlayer == null)
        {
            _musicPlayer = GetComponent<AudioSource>();
        }
        _musicPlayer.Stop();
        _currentMusicIndex = BAHMANMusicBox.INVALID_INDEX_NUMBER;
        _sceneMusicInfo = iSceneInfo;

        StartCoroutine(_PlaySceneList());
        yield return 0;
    }
    IEnumerator _PlaySceneList()
    {
        if (_musicPlayer.isPlaying)
        {
            if (_sceneMusicInfo._StopMusicGradually)
            {
                do
                {
                    _musicPlayer.volume -= _sceneMusicInfo._FadeInterval * Time.deltaTime;
                    yield return 0;
                } while (_musicPlayer.volume <= BAHMANMusicBox.MIN_VOLUME);
            }
            _musicPlayer.Stop();
            yield return new WaitForSeconds(_sceneMusicInfo._SilenceBetweenClips);
        }
        _currentMusicIndex = _nextIndext(_currentMusicIndex);

        if (_currentMusicIndex == BAHMANMusicBox.INVALID_INDEX_NUMBER)
        {
            StopAllCoroutines();

        }
        else
        {
            _musicPlayer.clip = _sceneMusicInfo._SceneMusics[_currentMusicIndex];
            _musicPlayer.volume = _sceneMusicInfo._GlobalMusicVolume;
            _musicPlayer.Play();
            if (BAHMANMessageBoxManager._INSTANCE.IsReady)
                BAHMANMessageBoxManager._INSTANCE._ShowMessage(_musicPlayer.clip.name, MESSAGEINTERVAL);
            OnMusicStarted?.Invoke(_sceneMusicInfo._SceneMusics[_currentMusicIndex].name);
            if (_sceneMusicInfo._StopMusicGradually)
            {
                StartCoroutine(_changeMusic(_musicPlayer.clip.length - _sceneMusicInfo._FadeInterval));
            }
            else
            {
                StartCoroutine(_changeMusic(_musicPlayer.clip.length));
            }
        }
        yield return 0;
    }
    IEnumerator _changeMusic(float iTime)
    {
        yield return new WaitForSeconds(iTime);
        StartCoroutine(_PlaySceneList());

    }


}

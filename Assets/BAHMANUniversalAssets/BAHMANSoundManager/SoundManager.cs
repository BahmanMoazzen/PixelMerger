using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager _Instance;
    [SerializeField] GameSoundStructure[] _sounds;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] GameSettingInfo _gameSetting;
    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);



    }
    private void OnEnable()
    {

        GameSettingInfo.OnSoundFXChange += GameSettingInfo_OnSoundFXChange;
        GameSettingInfo.OnMusicChange += GameSettingInfo_OnMusicChange;
    }

    private void GameSettingInfo_OnMusicChange(bool iEnable)
    {
        
    }

    private void OnDisable()
    {
        GameSettingInfo.OnSoundFXChange -= GameSettingInfo_OnSoundFXChange;
        GameSettingInfo.OnMusicChange -= GameSettingInfo_OnMusicChange;
    }

    private void GameSettingInfo_OnSoundFXChange(bool iEnable)
    {
        if (iEnable)
        {
            _audioSource.PlayOneShot(_sounds[(int)GameSounds.FirstMerge].AudioClips[0]);
        }

    }

    public void _PlaySound(GameSounds iSound)
    {
        if (_gameSetting.SoundFX)
        {
            foreach (var sound in _sounds)
            {
                if (sound.Sound == iSound)
                {
                    _audioSource.PlayOneShot(sound.AudioClips[Random.Range(0, sound.AudioClips.Count)]);
                }
            }
        }

    }

}



using System.Collections.Generic;
using UnityEngine;

public class BAHMANSoundManager : MonoBehaviour
{
    public static BAHMANSoundManager Instance;
    //[SerializeField] GameSoundStructure[] _sounds;
    [SerializeField] BAHMANSoundSettingInfo _setting;
    [SerializeField] AudioSource _audioSource;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);



    }
    private void OnEnable()
    {

        GameSettingInfo.OnSoundFXChange += GameSettingInfo_OnSoundFXChange;
        //GameSettingInfo.OnMusicChange += GameSettingInfo_OnMusicChange;
    }

    

    private void OnDisable()
    {
        GameSettingInfo.OnSoundFXChange -= GameSettingInfo_OnSoundFXChange;
        //GameSettingInfo.OnMusicChange -= GameSettingInfo_OnMusicChange;
    }

    private void GameSettingInfo_OnSoundFXChange(bool iEnable)
    {
        if (iEnable)
        {
            _audioSource.PlayOneShot(_setting.Sounds[(int)GameSounds.FirstMerge].AudioClips[0]);
        }

    }

    public void _PlaySound(GameSounds iSound)
    {
        if (A.GameSetting.SoundFX)
        {
            foreach (var sound in _setting.Sounds)
            {
                if (sound.Sound == iSound)
                {
                    _audioSource.PlayOneShot(sound.AudioClips[Random.Range(0, sound.AudioClips.Count)]);
                }
            }
        }

    }

}




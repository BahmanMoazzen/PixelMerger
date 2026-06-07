using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameSettingInfo _gameSettingInfo;
    [SerializeField] Toggle _soundFX, _antialiasing,_musicToggle;
    [SerializeField] GameObject _ratingBottom;
    
    private void Start()
    {
        _soundFX.isOn = _gameSettingInfo.SoundFX;
        //_antialiasing.isOn = _gameSettingInfo.AntiAliasing;
        _musicToggle.isOn = _gameSettingInfo.Music;
        //if (_gameSettingInfo.RatingURL == string.Empty)
        //{
        //    _ratingBottom.SetActive(false);
        //}
        //else
        //{
        //    _ratingBottom.SetActive(true);
        //}
    }
    /// <summary>
    /// Starts the game by loading the game scene. This method is called when the player clicks the "Start Game" button on the title screen.
    /// </summary>
    public void _StartGame()
    {
        
        BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.GameScene);
    }
    /// <summary>
    /// opens up the menu when the player clicks the "Back" button on the title screen. This method is called when the player clicks the "Back" button on the title screen.
    /// </summary>
    public void _BackButton()
    {
        BAHMANBackButtonManager._Instance._ShowMenu();
    }
    public void _SelectDeck(int iGameDifficulty)
    {
        //A.Levels.DifficultyLevel =(GameModes) iGameDifficulty;
        //SoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
        //BAHMANLoadingManager._INSTANCE._LoadScene(AllScenes.DeckSelect);

    }
    public void _ShowRewarded()
    {
        BAHMANAdManager.Instance._ShowRewardedAd();
    }
    public void _ShowInterstitial()
    {
        BAHMANAdManager.Instance._ShowInterstitialAd();
    }
    public void _OnSoundFXChanged()
    {
        _gameSettingInfo.SoundFX = _soundFX.isOn;
    }
    public void _OnMusicChanged()
    {
        _gameSettingInfo.Music = _musicToggle.isOn;
    }
    public void _OnAntialiasingChanged()
    {
        _gameSettingInfo.AntiAliasing = _antialiasing.isOn;

    }
    public void _RateUs()
    {
        SoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
        //Application.OpenURL(string.Format(_gameSettingInfo.RatingURL, Application.identifier));
    }

    public void _ButtomClicked()
    {
        SoundManager._Instance._PlaySound(GameSounds.ButtomClicked);
    }
}

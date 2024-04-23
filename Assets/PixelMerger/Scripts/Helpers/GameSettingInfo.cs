using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "NewSettings", order = 2, menuName = "Pixel Merger/New Game Settings")]
public class GameSettingInfo : ScriptableObject
{

    const string SOUNDFXTAG = "SoundFXTag", ANTIALIASINGTAG = "AntiAliasingTag",MUSICTAG = "MUSICTag";
    public static event UnityAction<bool> OnSoundFXChange;
    public static event UnityAction<bool> OnMusicChange;
    public static event UnityAction<bool> OnAntialiasingChange;

    public PixelDeckInfo[] AllDecks;
    public GameObject PixelSkeleton;
    public GameObject DeckButtonTemplate;
    public int[] PixelMergeScores = { 0, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048,4096 };
    public float[] PixelGizmoRadious = { 0, .5f, .77f, 1f, 1.26f, 1.5f, 1.77f, 2f, 2.26f, 2.5f, 2.77f, 3f, 3.26f };
    [SerializeField] bool antiAliasing = true;
    [SerializeField] bool soundFX;
    [SerializeField] bool music;

    public string RatingURL;

    public bool SoundFX
    {
        get
        {
            return A.Tools.IntToBool(PlayerPrefs.GetInt(SOUNDFXTAG, A.Tools.BoolToInt(soundFX)));

        }
        set
        {
            OnSoundFXChange?.Invoke(value);
            PlayerPrefs.SetInt(SOUNDFXTAG, A.Tools.BoolToInt(value));
        }
    }
    public bool Music
    {
        get
        {
            return A.Tools.IntToBool(PlayerPrefs.GetInt(MUSICTAG, A.Tools.BoolToInt(music)));

        }
        set
        {
            OnMusicChange?.Invoke(value);
            PlayerPrefs.SetInt(MUSICTAG, A.Tools.BoolToInt(value));
        }
    }

    public bool AntiAliasing
    {
        get
        {

            return A.Tools.IntToBool(PlayerPrefs.GetInt(ANTIALIASINGTAG, A.Tools.BoolToInt(antiAliasing)));

        }
        set
        {
            OnAntialiasingChange?.Invoke(value);
            PlayerPrefs.SetInt(ANTIALIASINGTAG, A.Tools.BoolToInt(value));
        }
    }
}



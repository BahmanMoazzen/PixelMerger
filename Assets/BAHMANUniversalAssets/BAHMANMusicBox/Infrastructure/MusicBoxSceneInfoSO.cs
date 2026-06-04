using UnityEngine;

[CreateAssetMenu(
    fileName = "MusicBoxSceneInfo",
    menuName = "BAHMAN Unity Assets/Music Box Scene Info",
    order = 1)]
public class MusicBoxSceneInfoSO : ScriptableObject
{
    [Tooltip("Scene build index for this config")]
    public int SceneBuildIndex;

    [Tooltip("All the scene's clips")]
    public AudioClip[] SceneMusics;

    [Tooltip("The global volume of music clips")]
    [Range(0f, 1f)]
    public float GlobalMusicVolume = 1f;

    [Tooltip("The style of choosing next clip")]
    public bool ShufflePlay;

    [Tooltip("Should music be stopped gradually")]
    public bool StopMusicGradually;

    [Tooltip("The amount of end of music clip to fade to silence")]
    [Range(.1f, 2f)]
    public float FadeInterval = 1f;

    [Tooltip("The amount of silence between music clips")]
    [Range(0f, 10f)]
    public float SilenceBetweenClips = 0f;

    [Tooltip("Only works on non-random play mode")]
    public bool StopAfterListEnded = false;

    private void OnValidate()
    {
        FadeInterval = Mathf.Max(0.1f, FadeInterval);
        SilenceBetweenClips = Mathf.Max(0f, SilenceBetweenClips);
        GlobalMusicVolume = Mathf.Clamp01(GlobalMusicVolume);
    }

    public MusicBoxSceneConfig ToDomain()
    {
        return new MusicBoxSceneConfig(
            SceneBuildIndex,
            SceneMusics,
            GlobalMusicVolume,
            ShufflePlay,
            StopMusicGradually,
            FadeInterval,
            SilenceBetweenClips,
            StopAfterListEnded);
    }
}

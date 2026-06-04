using UnityEngine;

[System.Serializable]
public class MusicBoxSceneConfig
{
    public int SceneBuildIndex { get; }
    public AudioClip[] Clips { get; }
    public float GlobalVolume { get; }
    public bool ShufflePlay { get; }
    public bool StopMusicGradually { get; }
    public float FadeInterval { get; }
    public float SilenceBetweenClips { get; }
    public bool StopAfterListEnded { get; }

    public MusicBoxSceneConfig(
        int sceneBuildIndex,
        AudioClip[] clips,
        float globalVolume,
        bool shufflePlay,
        bool stopMusicGradually,
        float fadeInterval,
        float silenceBetweenClips,
        bool stopAfterListEnded)
    {
        SceneBuildIndex = sceneBuildIndex;
        Clips = clips;
        GlobalVolume = Mathf.Clamp01(globalVolume);
        ShufflePlay = shufflePlay;
        StopMusicGradually = stopMusicGradually;
        FadeInterval = Mathf.Max(0.01f, fadeInterval);
        SilenceBetweenClips = Mathf.Max(0f, silenceBetweenClips);
        StopAfterListEnded = stopAfterListEnded;
    }

    public bool HasClips => Clips != null && Clips.Length > 0;
}

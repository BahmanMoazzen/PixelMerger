using UnityEngine;

public class MusicPlaylist
{
    public const int InvalidIndex = -1;

    private readonly MusicBoxSceneConfig _config;
    private int _currentIndex = InvalidIndex;

    public MusicPlaylist(MusicBoxSceneConfig config)
    {
        _config = config;
    }

    public int CurrentIndex => _currentIndex;

    public AudioClip GetNextClip()
    {
        if (!_config.HasClips)
        {
            _currentIndex = InvalidIndex;
            return null;
        }

        if (_config.ShufflePlay)
        {
            int next;
            if (_config.Clips.Length == 1)
            {
                next = 0;
            }
            else
            {
                do
                {
                    next = Random.Range(0, _config.Clips.Length);
                } while (next == _currentIndex);
            }

            _currentIndex = next;
        }
        else
        {
            int next = _currentIndex + 1;
            if (next >= _config.Clips.Length)
            {
                if (_config.StopAfterListEnded)
                {
                    _currentIndex = InvalidIndex;
                    return null;
                }
                next = 0;
            }
            _currentIndex = next;
        }

        return _config.Clips[_currentIndex];
    }

    public bool IsFinished => _currentIndex == InvalidIndex;
}

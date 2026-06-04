using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MusicBoxConfigRepository",
    menuName = "BAHMAN Unity Assets/Music Box Config Repository",
    order = 2)]
public class ScriptableObjectMusicBoxConfigRepository : ScriptableObject, IMusicBoxConfigRepository
{
    [SerializeField]
    private MusicBoxSceneInfoSO[] _sceneInfos;

    private Dictionary<int, MusicBoxSceneConfig> _lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

    private void BuildLookup()
    {
        _lookup = new Dictionary<int, MusicBoxSceneConfig>();
        if (_sceneInfos == null) return;

        foreach (var info in _sceneInfos)
        {
            if (info == null) continue;
            var config = info.ToDomain();
            _lookup[config.SceneBuildIndex] = config;
        }
    }

    public bool TryGetSceneConfig(int sceneBuildIndex, out MusicBoxSceneConfig config)
    {
        if (_lookup == null)
            BuildLookup();

        return _lookup.TryGetValue(sceneBuildIndex, out config);
    }
}

public interface IMusicBoxConfigRepository
{
    bool TryGetSceneConfig(int sceneBuildIndex, out MusicBoxSceneConfig config);
}

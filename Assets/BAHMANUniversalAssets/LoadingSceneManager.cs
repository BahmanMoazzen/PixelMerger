using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] AllScenes _nextScene;
    private void Awake()
    {
        SceneManager.LoadScene((int)_nextScene);
    }


}

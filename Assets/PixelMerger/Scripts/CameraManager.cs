using UnityEngine;

public class CameraManager : MonoBehaviour
{
    int _magnetude = 0;
    Animator _animator;
    private void OnEnable()
    {
        MergersController.OnMerge += MergersController_OnMerge;
    }

    private void MergersController_OnMerge(GameObject iOldPixel1, GameObject iOldPixel2, GameObject iNewPixel)
    {
        
    }

    private void OnDisable()
    {
        MergersController.OnMerge -= MergersController_OnMerge;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();

    }
}

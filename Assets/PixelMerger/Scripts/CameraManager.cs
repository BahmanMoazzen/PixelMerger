using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameSettingInfo _gameSettingInfo;
    Animator _animator;
    private void OnEnable()
    {
        GameSettingInfo.OnAntialiasingChange += GameSettingInfo_OnAntialiasingChange;
    }

    private void GameSettingInfo_OnAntialiasingChange(bool iEnable)
    {
        _animator.SetBool("AntiAliasing", iEnable);
    }

    private void OnDisable()
    {
        GameSettingInfo.OnAntialiasingChange -= GameSettingInfo_OnAntialiasingChange;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("AntiAliasing", _gameSettingInfo.AntiAliasing);

    }
}

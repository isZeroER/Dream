using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumePanel : BasePanel
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        slider.value = AudioManager.Instance.GetBGMVolume();   
    }

    private void Start()
    {
        slider.onValueChanged.AddListener(UpdateVolume);
        backButton.onClick.AddListener(Back);
    }

    private void UpdateVolume(float currentVolume)
    {
        AudioManager.Instance.SetBGMVolume(currentVolume);
    }

    private void Back() => Close();
}

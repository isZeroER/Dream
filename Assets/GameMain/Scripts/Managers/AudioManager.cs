using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource ForBGM;
    [SerializeField] private List<AudioClip> bgmLists = new List<AudioClip>();

    [SerializeField] private List<AudioSource> ForFXs;

    private void Start()
    {
        ForBGM.loop = true;
        PlayBGM(0);
    }

    public void PlayFX(int id)
    {
        ForFXs[id].Play();
    }

    public void PlayBGM(int id)
    {
        ForBGM.Stop();
        ForBGM.clip = bgmLists[id];
        ForBGM.Play();
    }
}

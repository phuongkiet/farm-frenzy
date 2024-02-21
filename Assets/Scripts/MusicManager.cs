using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] AudioSource audioSource;
    [SerializeField] float timeToSwitch;
    [SerializeField] AudioClip audioClip;
    [SerializeField] AudioClip audioClip2;
    private float gameTime = 0f;
    private void Start()
    {
        Play(audioClip, true);
    }
    public void Play(AudioClip musicToPlay, bool interupt = false)
    {
        if(musicToPlay == null) { return; }
        if(interupt == true)
        {
            audioSource.volume = 1f;
            audioSource.clip = musicToPlay;
            audioSource.Play();
        }
        else
        {
            switchTo = musicToPlay;
            StartCoroutine(SmoothSwitchMusic());
        }
    }

    AudioClip switchTo;
    float volume;
    IEnumerator SmoothSwitchMusic()
    {
        volume = 1f;
        while(volume > 0f)
        {
            volume -= Time.deltaTime / timeToSwitch;
            if(volume < 0f) { volume = 0f; }
            audioSource.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        Play(switchTo, true);
    }

    public void SwitchToNightMusic()
    {
        SmoothSwitchMusic();
        Play(audioClip2, true);
    }

    internal void SwitchToDayMusic()
    {
        SmoothSwitchMusic();
        Play(audioClip, true);
    }
}

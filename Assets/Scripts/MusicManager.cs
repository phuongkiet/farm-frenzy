using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Slider volumeSlider;
    private float gameTime = 0f;
    private void Start()
    {
        
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void Play(AudioClip musicToPlay, bool interupt = false)
    {
        if(musicToPlay == null) { return; }
        if(interupt == true)
        {
            /*audioSource.volume = 1f;*/
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

    public void ChangeVolume()
    {
        audioSource.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        audioSource.volume = volumeSlider.value; 
        Play(audioClip, true);
    }


    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}

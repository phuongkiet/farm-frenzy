using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] GameObject audioSourcePrefab;
    [SerializeField] int audioSourceCount;
    [SerializeField] Slider volumeSlider;

    List<AudioSource> audioSources;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < audioSourceCount; i++)
        {
            GameObject go = Instantiate(audioSourcePrefab, transform);
            go.transform.localPosition = Vector3.zero;
            AudioSource audioSource = go.GetComponent<AudioSource>();
            audioSources.Add(audioSource);
        }

        Load(); // Load the volume setting at the start
    }

    public void Play(AudioClip audioClip)
    {
        if (audioClip == null) { return; }
        AudioSource audioSource = GetFreeAudioSource();

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying == false)
            {
                return audioSources[i];
            }
        }
        return audioSources[0];
    }

    public void ChangeVolume()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volumeSlider.value;
        }
        Save(); // Save the volume setting whenever it changes
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("audioVolume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("audioVolume");
            ChangeVolume(); // Update the volume of the audio sources
        }
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("audioVolume", volumeSlider.value);
    }
}

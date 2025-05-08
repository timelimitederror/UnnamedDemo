using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSinqletonBase<AudioManager>
{
    private const float DEFAULT_BGM_VOLUME = 0.2f;
    private const float DEFAULT_SOUND_VOLULME = 0.5f;

    private AudioSource bgmAudioSource;
    private List<AudioSource> soundAudioSources = new List<AudioSource>();
    private float bgmVolume = DEFAULT_BGM_VOLUME;
    private float soundVolume = DEFAULT_SOUND_VOLULME;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            bgmAudioSource.volume = bgmVolume;
        }
    }
    public float SoundVolume
    {
        get
        {
            return soundVolume;
        }
        set
        {
            soundVolume = value;
            foreach (AudioSource audioSource in soundAudioSources)
            {
                audioSource.volume = soundVolume;
            }
        }
    }

    private AudioClip loginBGM;

    void Start()
    {
        bgmAudioSource = GetComponent<AudioSource>();
        bgmAudioSource.volume = bgmVolume;
    }

    public void playBGM(AudioClip audioClip)
    {
        bgmAudioSource.clip = audioClip;
        bgmAudioSource.Play();
    }

    public void stopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void playBGM()
    {
        bgmAudioSource.Play();
    }

    public void bgmMute()
    {
        bgmAudioSource.mute = true;
    }

    public void bgmUnmute()
    {
        bgmAudioSource.mute = false;
    }

    public void soundMute()
    {
        foreach (AudioSource audioSource in soundAudioSources)
        {
            audioSource.mute = true;
        }
    }

    public void soundUnmute()
    {
        foreach (AudioSource audioSource in soundAudioSources)
        {
            audioSource.mute = false;
        }
    }

    public void addSoundAudioSource(AudioSource audioSource)
    {
        audioSource.volume = soundVolume;
        soundAudioSources.Add(audioSource);
    }

    public void removeSoundAudioSource(AudioSource audioSource)
    {
        soundAudioSources.Remove(audioSource);
    }
}

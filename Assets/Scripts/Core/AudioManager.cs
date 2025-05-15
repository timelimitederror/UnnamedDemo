using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSinqletonBase<AudioManager>
{
    private const float DEFAULT_BGM_VOLUME = 0.2f;
    private const float DEFAULT_SOUND_VOLULME = 0.5f;

    private AudioSource activeBgmAudioSource;
    private AudioSource bgmAS01;
    private AudioSource bgmAS02;
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
            if (activeBgmAudioSource != null)
            {
                activeBgmAudioSource.volume = bgmVolume;
            }
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

    // private AudioClip loginBGM;
    private float fadeDuration = 1f; // 切换bgm淡入淡出的时间
    private Coroutine currentFadeRoutine; // 加载bgm的协程

    void Start()
    {
        //bgmAudioSource = GetComponent<AudioSource>();
        //bgmAudioSource.volume = bgmVolume;
        bgmAS01 = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        bgmAS02 = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        bgmAS01.volume = 0;
        bgmAS02.volume = 0;
        activeBgmAudioSource = bgmAS01;
    }

    public void playBGM(AudioClip audioClip)
    {
        //bgmAudioSource.clip = audioClip;
        //bgmAudioSource.Play();
        if (activeBgmAudioSource.clip == audioClip && activeBgmAudioSource.isPlaying) // 正在播放相同bgm的话就不理
        {
            return;
        }

        if (currentFadeRoutine != null) // 停止正在进行的淡入淡出
        {
            StopCoroutine(currentFadeRoutine);
        }

        currentFadeRoutine = StartCoroutine(BGMFadeRoutine(audioClip)); // 协程切换bgm
    }

    private IEnumerator BGMFadeRoutine(AudioClip audioClip)
    {
        AudioSource oldSource = activeBgmAudioSource;
        AudioSource newSource = activeBgmAudioSource == bgmAS01 ? bgmAS02 : bgmAS01;

        // 载入新音频 过大的音频需要在音频文件的检查器界面勾选上预加载音频数据，不然会在Play()的时候解析音频文件，直接卡帧（可能耗时200-300ms)
        //Debug.Log("mark3: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        newSource.clip = audioClip;
        newSource.volume = 0;
        //long m1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //Debug.Log("mark1: " + m1);
        newSource.Play();
        //long m2 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //Debug.Log("mark2: " + m2);
        //Debug.Log("m2-m1: " + (m2 - m1));
        float timer = 0f;

        while (timer <= fadeDuration)
        {
            // 线性插值音量
            float progress = timer / fadeDuration;
            newSource.volume = Mathf.Lerp(0, bgmVolume, progress);
            oldSource.volume = Mathf.Lerp(bgmVolume, 0, progress);

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
        newSource.volume = bgmVolume;
        oldSource.volume = 0;
        oldSource.Stop();

        // 切换活跃音源
        activeBgmAudioSource = newSource;
    }

    public void stopBGM()
    {
        activeBgmAudioSource.Stop();
    }

    public void playBGM()
    {
        activeBgmAudioSource.Play();
    }

    public void bgmMute()
    {
        bgmAS01.mute = true;
        bgmAS02.mute = true;
    }

    public void bgmUnmute()
    {
        bgmAS01.mute = false;
        bgmAS02.mute = false;
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

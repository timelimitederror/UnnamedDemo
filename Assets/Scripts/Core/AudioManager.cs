using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSinqletonBase<AudioManager>
{
    private const float DEFAULT_BGM_VOLUME = 0.2f;
    private const float DEFAULT_SOUND_VOLULME = 0.5f;
    private const string INIT_NAME = "voice";

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
            init["bgmVolume"] = value;
            InitManager.Instance.ReplaceInit(INIT_NAME, init);
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
            init["soundVolume"] = value;
            InitManager.Instance.ReplaceInit(INIT_NAME, init);
            foreach (AudioSource audioSource in soundAudioSources)
            {
                audioSource.volume = soundVolume;
            }
        }
    }

    public bool BGMIsMute
    {
        get
        {
            return bgmAS01.mute;
        }
    }

    private bool soundIsMute = false;
    public bool SoundIsMute
    {
        get
        {
            return soundIsMute;
        }
    }

    // private AudioClip loginBGM;
    private float fadeDuration = 1f; // �л�bgm���뵭����ʱ��
    private Coroutine currentFadeRoutine; // ����bgm��Э��
    private JObject init;

    public override void Awake()
    {
        base.Awake();

        bgmAS01 = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        bgmAS02 = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        bgmAS01.volume = bgmVolume;
        bgmAS02.volume = bgmVolume;
        activeBgmAudioSource = bgmAS01;

        // ��ʼ������
        JObject init = (JObject)InitManager.Instance.GetInitByName(INIT_NAME);
        this.init = init;
        bgmVolume = (float)init["bgmVolume"];
        bgmAS01.volume = bgmVolume;
        bgmAS02.volume = bgmVolume;
        soundVolume = (float)init["soundVolume"];
        bool bgmIsMute = (bool)init["bgmIsMute"];
        if (bgmIsMute)
        {
            bgmMute();
        }
        else
        {
            bgmUnmute();
        }
        soundIsMute = (bool)init["soundIsMute"];
    }

    void Start()
    {
        //bgmAudioSource = GetComponent<AudioSource>();
        //bgmAudioSource.volume = bgmVolume;
    }

    public void playBGM(AudioClip audioClip)
    {
        //bgmAudioSource.clip = audioClip;
        //bgmAudioSource.Play();
        if (activeBgmAudioSource.clip == audioClip && activeBgmAudioSource.isPlaying) // ���ڲ�����ͬbgm�Ļ��Ͳ���
        {
            return;
        }

        if (currentFadeRoutine != null) // ֹͣ���ڽ��еĵ��뵭��
        {
            StopCoroutine(currentFadeRoutine);
        }

        currentFadeRoutine = StartCoroutine(BGMFadeRoutine(audioClip)); // Э���л�bgm
    }

    private IEnumerator BGMFadeRoutine(AudioClip audioClip)
    {
        AudioSource oldSource = activeBgmAudioSource;
        AudioSource newSource = activeBgmAudioSource == bgmAS01 ? bgmAS02 : bgmAS01;

        // ��������Ƶ �������Ƶ��Ҫ����Ƶ�ļ��ļ�������湴ѡ��Ԥ������Ƶ���ݣ���Ȼ����Play()��ʱ�������Ƶ�ļ���ֱ�ӿ�֡�����ܺ�ʱ200-300ms)
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
            // ���Բ�ֵ����
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

        // �л���Ծ��Դ
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
        init["bgmIsMute"] = true;
        InitManager.Instance.ReplaceInit(INIT_NAME, init);
        bgmAS01.mute = true;
        bgmAS02.mute = true;
    }

    public void bgmUnmute()
    {
        init["bgmIsMute"] = false;
        InitManager.Instance.ReplaceInit(INIT_NAME, init);
        bgmAS01.mute = false;
        bgmAS02.mute = false;
    }

    public void soundMute()
    {
        soundIsMute = true;
        init["soundIsMute"] = true;
        InitManager.Instance.ReplaceInit(INIT_NAME, init);
        foreach (AudioSource audioSource in soundAudioSources)
        {
            audioSource.mute = true;
        }
    }

    public void soundUnmute()
    {
        soundIsMute = false;
        init["soundIsMute"] = false;
        InitManager.Instance.ReplaceInit(INIT_NAME, init);
        foreach (AudioSource audioSource in soundAudioSources)
        {
            audioSource.mute = false;
        }
    }

    public void addSoundAudioSource(AudioSource audioSource)
    {
        audioSource.volume = soundVolume;
        audioSource.mute = soundIsMute;
        soundAudioSources.Add(audioSource);
    }

    public void removeSoundAudioSource(AudioSource audioSource)
    {
        soundAudioSources.Remove(audioSource);
    }
}

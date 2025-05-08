using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceController : MonoBehaviour
{
    public GameObject bgImame;
    public GameObject panel;

    public Slider bgmSlider;
    public Slider soundSlider;

    public GameObject bgmMute;
    public GameObject bgmUnmute;
    public GameObject soundMute;
    public GameObject soundUnmute;

    private bool isBgmMute = false;
    private bool isSoundMute = false;

    void Start()
    {
        bgmSlider.value = AudioManager.Instance.BgmVolume;
        soundSlider.value = AudioManager.Instance.SoundVolume;
    }

    public void open()
    {
        bgImame.SetActive(true);
        panel.SetActive(true);
    }

    public void close()
    {
        bgImame.SetActive(false);
        panel.SetActive(false);
    }

    public void changeBgmVolume()
    {
        if (!isBgmMute)
        {
            AudioManager.Instance.BgmVolume = bgmSlider.value;
        }
    }

    public void changeSoundVolume()
    {
        if (!isSoundMute)
        {
            AudioManager.Instance.SoundVolume = soundSlider.value;
        }
    }

    public void setBgmMute()
    {
        AudioManager.Instance.bgmMute();

        isBgmMute = true;
        bgmSlider.value = 0;
        bgmSlider.interactable = false;
        bgmMute.SetActive(true);
        bgmUnmute.SetActive(false);
    }

    public void setBgmUnmute()
    {
        AudioManager.Instance.bgmUnmute();

        isBgmMute = false;
        bgmSlider.interactable = true;
        bgmSlider.value = AudioManager.Instance.BgmVolume;
        bgmMute.SetActive(false);
        bgmUnmute.SetActive(true);
    }

    public void setSoundMute()
    {
        AudioManager.Instance.soundMute();

        isSoundMute = true;
        soundSlider.value = 0;
        soundSlider.interactable = false;
        soundMute.SetActive(true);
        soundUnmute.SetActive(false);
    }

    public void setSoundUnmute()
    {
        AudioManager.Instance.soundUnmute();

        isSoundMute = false;
        soundSlider.value = AudioManager.Instance.SoundVolume;
        soundSlider.interactable = true;
        soundMute.SetActive(false);
        soundUnmute.SetActive(true);
    }
}

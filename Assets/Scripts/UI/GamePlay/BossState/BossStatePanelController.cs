using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatePanelController : MonoBehaviour
{
    public BossHealthController mixingHealth;
    public BossHealthController redHealth;
    public BossHealthController greenHealth;
    public BossHealthController blueHealth;
    public GameObject bossName;

    private Action<PolluroRGBHealthChanged> polluroRGBHealthChangedAction;
    private Action<PolluroMixingHealthChanged> polluroMixingHealthChangedAction;

    private bool isRGB = true;

    void Start()
    {
        showRGBHealth();

        polluroRGBHealthChangedAction = new Action<PolluroRGBHealthChanged>(changeRGBHealth);
        polluroMixingHealthChangedAction = new Action<PolluroMixingHealthChanged>(changeMixingHealth);

        EventBus.Subscribe(polluroRGBHealthChangedAction);
        EventBus.Subscribe(polluroMixingHealthChangedAction);
    }

    private void changeRGBHealth(PolluroRGBHealthChanged polluroEvent)
    {
        if (!isRGB)
        {
            showRGBHealth();
        }
        redHealth.setHealthValue(polluroEvent.redValue, polluroEvent.maxRedValue);
        greenHealth.setHealthValue(polluroEvent.greenValue, polluroEvent.maxGreenValue);
        blueHealth.setHealthValue(polluroEvent.blueValue, polluroEvent.maxBlueValue);
    }

    private void changeMixingHealth(PolluroMixingHealthChanged polluroEvent)
    {
        if (isRGB)
        {
            showMixingHealth();
        }
        mixingHealth.setHealthValue(polluroEvent.mixingValue, polluroEvent.maxMixingValue);
    }

    private void showMixingHealth()
    {
        redHealth.gameObject.SetActive(false);
        greenHealth.gameObject.SetActive(false);
        blueHealth.gameObject.SetActive(false);
        mixingHealth.gameObject.SetActive(true);
        bossName.gameObject.SetActive(true);
        isRGB = false;
    }

    private void showRGBHealth()
    {
        mixingHealth.gameObject.SetActive(false);
        redHealth.gameObject.SetActive(true);
        greenHealth.gameObject.SetActive(true);
        blueHealth.gameObject.SetActive(true);
        bossName.gameObject.SetActive(true);
        isRGB = true;
    }

    private void closeAllPanel()
    {
        redHealth.gameObject.SetActive(false);
        greenHealth.gameObject.SetActive(false);
        blueHealth.gameObject.SetActive(false);
        mixingHealth.gameObject.SetActive(true);
        bossName.gameObject.SetActive(false);
    }
}

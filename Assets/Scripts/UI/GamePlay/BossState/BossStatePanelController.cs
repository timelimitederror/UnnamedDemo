using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatePanelController : MonoBehaviour
{
    public BossHealthController redHealth;
    public BossHealthController greenHealth;
    public BossHealthController blueHealth;
    public GameObject bossName;

    private Action<PolluroRGBHealthChanged> polluroRGBHealthChangedAction;
    private Action<GameScene01Victory> gameScene01VictoryAction;

    void Start()
    {
        showRGBHealth();

        polluroRGBHealthChangedAction = new Action<PolluroRGBHealthChanged>(changeRGBHealth);
        gameScene01VictoryAction = new Action<GameScene01Victory>(victoryEvent =>
        {
            closeAllPanel();
        });

        EventBus.Subscribe(polluroRGBHealthChangedAction);
        EventBus.Subscribe(gameScene01VictoryAction);
    }

    void OnEnable()
    {
        showRGBHealth();
    }

    private void changeRGBHealth(PolluroRGBHealthChanged polluroEvent)
    {
        redHealth.setHealthValue(polluroEvent.redValue, polluroEvent.maxRedValue);
        greenHealth.setHealthValue(polluroEvent.greenValue, polluroEvent.maxGreenValue);
        blueHealth.setHealthValue(polluroEvent.blueValue, polluroEvent.maxBlueValue);
    }

    private void showRGBHealth()
    {
        redHealth.gameObject.SetActive(true);
        greenHealth.gameObject.SetActive(true);
        blueHealth.gameObject.SetActive(true);
        bossName.gameObject.SetActive(true);
    }

    private void closeAllPanel()
    {
        redHealth.gameObject.SetActive(false);
        greenHealth.gameObject.SetActive(false);
        blueHealth.gameObject.SetActive(false);
        bossName.gameObject.SetActive(false);
    }
}

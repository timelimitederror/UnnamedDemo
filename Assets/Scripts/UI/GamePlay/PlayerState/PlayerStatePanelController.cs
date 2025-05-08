using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePanelController : MonoBehaviour
{
    public ColorValueController mixingColor;
    public ColorValueController redColor;
    public ColorValueController greenColor;
    public ColorValueController blueColor;
    public HealthController health;
    public StaminaController stamina;

    private Action<PlayerColorValueChanged> playerStatePanelControllerAction;
    private Action<PlayerHealthChanged> playerHealthChangedAction;
    private Action<PlayerStaminaChanged> playerStaminaChangedAction;

    // Start is called before the first frame update
    void Start()
    {
        playerStatePanelControllerAction = new Action<PlayerColorValueChanged>(changeColorValue);
        playerHealthChangedAction = new Action<PlayerHealthChanged>(changeHealth);
        playerStaminaChangedAction = new Action<PlayerStaminaChanged>(changeStamina);

        EventBus.Subscribe(playerStatePanelControllerAction);
        EventBus.Subscribe(playerHealthChangedAction);
        EventBus.Subscribe(playerStaminaChangedAction);

    }

    private void changeColorValue(PlayerColorValueChanged playerEvent)
    {
        mixingColor.setColorValue(playerEvent.mixingValue, playerEvent.maxMixingValue);
        redColor.setColorValue(playerEvent.redValue, playerEvent.maxRedValue);
        greenColor.setColorValue(playerEvent.greenValue, playerEvent.maxGreenValue);
        blueColor.setColorValue(playerEvent.blueValue, playerEvent.maxBlueValue);
    }

    private void changeHealth(PlayerHealthChanged playerEvent)
    {
        health.setHealthValue(playerEvent.healthValue, playerEvent.maxHealthValue);
    }

    private void changeStamina(PlayerStaminaChanged playerEvent)
    {
        stamina.setStaminaValue(playerEvent.staminaValue, playerEvent.maxStaminaValue);
    }
}

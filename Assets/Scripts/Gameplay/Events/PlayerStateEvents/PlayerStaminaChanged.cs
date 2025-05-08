using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaChanged
{
    public float staminaValue;
    public float maxStaminaValue;

    public PlayerStaminaChanged(float staminaValue, float maxStaminaValue)
    {
        this.staminaValue = staminaValue;
        this.maxStaminaValue = maxStaminaValue;
    }
}

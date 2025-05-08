using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChanged
{
    public int healthValue;
    public int maxHealthValue;

    public PlayerHealthChanged(int healthValue, int maxHealthValue)
    {
        this.healthValue = healthValue;
        this.maxHealthValue = maxHealthValue;
    }
}

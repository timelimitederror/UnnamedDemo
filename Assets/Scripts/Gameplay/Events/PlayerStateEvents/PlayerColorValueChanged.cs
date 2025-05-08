using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorValueChanged
{
    public int mixingValue;
    public int maxMixingValue;
    public int redValue;
    public int greenValue;
    public int blueValue;
    public int maxRedValue;
    public int maxGreenValue;
    public int maxBlueValue;

    public PlayerColorValueChanged(
        int mixingValue, int maxMixingValue, 
        int redValue, int maxRedValue, 
        int greenValue, int maxGreenValue, 
        int blueValue, int maxBlueValue)
    {
        this.mixingValue = mixingValue;
        this.maxMixingValue = maxMixingValue;
        this.redValue = redValue;
        this.greenValue = greenValue;
        this.blueValue = blueValue;
        this.maxRedValue = maxRedValue;
        this.maxGreenValue = maxGreenValue;
        this.maxBlueValue = maxBlueValue;
    }
}

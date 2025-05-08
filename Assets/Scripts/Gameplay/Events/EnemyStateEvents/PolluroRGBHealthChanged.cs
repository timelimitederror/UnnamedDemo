using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroRGBHealthChanged
{
    public int redValue;
    public int maxRedValue;
    public int greenValue;
    public int maxGreenValue;
    public int blueValue;
    public int maxBlueValue;

    public PolluroRGBHealthChanged(
        int redValue, int maxRedValue,
        int greenValue, int maxGreenValue,
        int blueValue, int maxBlueValue)
    {
        this.redValue = redValue;
        this.maxRedValue = maxRedValue;
        this.greenValue = greenValue;
        this.maxGreenValue = maxGreenValue;
        this.blueValue = blueValue;
        this.maxBlueValue = maxBlueValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviourSinqletonBase<TimeManager>
{
    private bool flow = true;

    public void stopTime()
    {
        Time.timeScale = 0f;
        flow = false;
    }

    public void startTime()
    {
        Time.timeScale = 1f;
        flow = true;
    }

    public bool timeFlow()
    {
        return flow;
    }
}

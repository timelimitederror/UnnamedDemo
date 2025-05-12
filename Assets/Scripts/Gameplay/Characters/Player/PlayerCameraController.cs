using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineFreeLook normalCamera;
    public CinemachineFreeLook fPCamera;

    private KeyCode switchKey = KeyCode.Mouse1;

    // Update is called once per frame
    void Update()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        if (Input.GetKeyDown(switchKey))
        {
            fPCamera.m_XAxis.Value = normalCamera.m_XAxis.Value;
            normalCamera.Priority = 9;
        }
        if (Input.GetKeyUp(switchKey))
        {
            normalCamera.m_XAxis.Value = fPCamera.m_XAxis.Value;
            normalCamera.Priority = 11;
        }
    }
}

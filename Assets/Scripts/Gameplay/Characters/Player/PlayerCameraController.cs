using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineFreeLook normalCamera;
    public CinemachineFreeLook fPCamera;

    private KeyCode switchKey = KeyCode.Mouse1;
    private PlayerStateController playerStateController;

    private bool isDisable = false;

    void Start()
    {
        playerStateController = GetComponent<PlayerStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        if (playerStateController.IsDie)
        {
            if (!isDisable)
            {
                DisableInput();
                isDisable = true;
            }
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

    private void DisableInput()
    {
        // 清除输入轴名称
        normalCamera.m_XAxis.m_InputAxisName = "";
        normalCamera.m_YAxis.m_InputAxisName = "";

        fPCamera.m_XAxis.m_InputAxisName = "";
        fPCamera.m_YAxis.m_InputAxisName = "";
    }

    private void EnableInput()
    {
        // 恢复默认输入轴（假设原本是 "Mouse X" 和 "Mouse Y"）
        normalCamera.m_XAxis.m_InputAxisName = "Mouse X";
        normalCamera.m_YAxis.m_InputAxisName = "Mouse Y";

        fPCamera.m_XAxis.m_InputAxisName = "Mouse X";
        fPCamera.m_YAxis.m_InputAxisName = "Mouse Y";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviourSinqletonBase<SystemManager>
{
    public void closeGame()
    {
        // �༭����ֹͣ����ģʽ
        //UnityEditor.EditorApplication.isPlaying = false;
        //��ʽ����ʱ�رճ���
        Application.Quit();
    }
}

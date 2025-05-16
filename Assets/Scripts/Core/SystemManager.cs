using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviourSinqletonBase<SystemManager>
{
    public void closeGame()
    {
        // 编辑器中停止播放模式
        //UnityEditor.EditorApplication.isPlaying = false;
        //正式构建时关闭程序
        Application.Quit();
    }
}

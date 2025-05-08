using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourSinqletonBase<UIManager>
{

    private Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();
    private AudioSource uiAudioSource;

    private KeyCode menu = KeyCode.Escape;

    // Start is called before the first frame update
    void Start()
    {
        uiAudioSource = GetComponent<AudioSource>();
        AudioManager.Instance.addSoundAudioSource(uiAudioSource);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject gameObject = transform.GetChild(i).gameObject;
            panelDictionary[gameObject.name] = gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKey(menu) && !SceneManager.GetActiveScene().name.Equals("LoginScene"))
        {
            openPanel("MenuPanel");
        }
    }

    public void openPanel(string panelName)
    {
        if (panelDictionary.ContainsKey(panelName))
        {
            panelDictionary[panelName].SetActive(true);
        }
    }

    public void closePanel(string panelName)
    {
        if (panelDictionary.ContainsKey(panelName))
        {
            panelDictionary[panelName].SetActive(false);
        }
    }

    public void registerPanel(GameObject panel)
    {
        panelDictionary[panel.name] = panel;
    }

    // 开始游戏 加载GameScene01场景
    public void startGame()
    {
        foreach (string panelName in panelDictionary.Keys)
        {
            panelDictionary[panelName].SetActive(false);
        }
        SceneManager.LoadScene("GameScene01");
        panelDictionary["PlayerPanel"].SetActive(true);
    }

    // 退出到开始菜单
    public void returnLoginScene()
    {
        foreach (string panelName in panelDictionary.Keys)
        {
            panelDictionary[panelName].SetActive(false);
        }
        SceneManager.LoadScene("LoginScene");
        panelDictionary["LoginPanel"].SetActive(true);
    }

    public void playOneShot(AudioClip clip)
    {
        uiAudioSource.PlayOneShot(clip);
    }
}

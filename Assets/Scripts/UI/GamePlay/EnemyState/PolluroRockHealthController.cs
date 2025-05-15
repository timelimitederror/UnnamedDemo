using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PolluroRockHealthController : MonoBehaviour
{
    public GameObject rockHealthModel;

    private Dictionary<GameObject, RockHealthUI> models = new Dictionary<GameObject, RockHealthUI>();
    private ObjectPool<GameObject> pool;
    private Action<PolluroRockAddUI> createAction;
    private Action<PolluroRockHealthChanged> healthAction;
    private Action<PolluroRockDestroy> destroyAction;

    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<GameObject>(
            () =>
            {
                GameObject obj = Instantiate(rockHealthModel);
                obj.transform.SetParent(transform, false);
                return obj;
            }, obj =>
            {
                obj.SetActive(true);
            }, obj =>
            {
                obj.SetActive(false);
            }, obj =>
            {
                Destroy(obj);
            }, true, 10, 100);

        createAction = new Action<PolluroRockAddUI>(rockEvent =>
        {
            if (!models.ContainsKey(rockEvent.gameObject))
            {
                models[rockEvent.gameObject] = new RockHealthUI(pool.Get());
            }
        });

        healthAction = new Action<PolluroRockHealthChanged>(rockEvent =>
        {
            if (!models.ContainsKey(rockEvent.gameObject))
            {
                return;
            }

            GameObject uiObject = models[rockEvent.gameObject].gameObject;
            // 检查坐标是否在镜头范围内
            // 将position对应到UI面板的坐标
            Vector3 pos = Camera.main.WorldToScreenPoint(rockEvent.position + Vector3.up);
            if (pos.z < 0)
            {
                uiObject.SetActive(false);
            }
            else
            {
                uiObject.SetActive(true);
                uiObject.transform.position = pos;
                models[rockEvent.gameObject].slider.normalizedValue = (float)rockEvent.health / (float)rockEvent.maxHealth;
            }
        });

        destroyAction = new Action<PolluroRockDestroy>(rockEvent =>
        {
            //Debug.Log(models.ContainsKey(rockEvent.gameObject));
            // 这里有个bug，当普攻击杀石块时，有概率血量UI无法setActive成功，但是pool回收成功，不知道为啥
            // 找到问题了，是事件总线的先后时间差。这边刚destroy完，那边还在调用health。将创建UI和刷新UI的事件分开来就可以了。
            if (models.ContainsKey(rockEvent.gameObject))
            {
                if (models[rockEvent.gameObject].gameObject != null)
                {
                    models[rockEvent.gameObject].gameObject.SetActive(false);
                    pool.Release(models[rockEvent.gameObject].gameObject);
                }
                models.Remove(rockEvent.gameObject);
                //Debug.Log(pool.CountActive);
            }
        });

        EventBus.Subscribe(createAction);
        EventBus.Subscribe(healthAction);
        EventBus.Subscribe(destroyAction);
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        pool.Clear();
    }

    private class RockHealthUI
    {
        public GameObject gameObject;
        public Slider slider;
        public RectTransform rectTransform;

        public RockHealthUI(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.slider = gameObject.GetComponent<Slider>();
            this.rectTransform = gameObject.GetComponent<RectTransform>();
        }
    }
}

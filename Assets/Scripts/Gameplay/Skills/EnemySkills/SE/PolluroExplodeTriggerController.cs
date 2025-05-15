using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroExplodeTriggerController : TriggerController
{
    private const float DELAY_TIME = 4f;
    private float startTime = 0f;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private bool isBoom = false;

    // enable可能在start之前执行 放在awake可以保证最先执行
    void Awake()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystem ps = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particleSystems.Add(ps);
            }
        }
    }

    // 继承不会触发父类中重复实现的，比如这个OnEnable，父类也有但是实际只运行子类的
    void OnEnable()
    {
        lastActiveTime = Time.fixedTime;
        startTime = Time.fixedTime + DELAY_TIME;
        isBoom = false;
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    void Update()
    {
        if (startTime <= Time.fixedTime && isBoom == false)
        {
            isBoom = true;
            Collider[] results = Physics.OverlapSphere(transform.position, 8f, LayerMask.GetMask("Player"));
            if (results.Length > 0)
            {
                foreach (Collider col in results)
                {
                    base.OnTriggerEnter(col);
                }
            }
        }
    }
}

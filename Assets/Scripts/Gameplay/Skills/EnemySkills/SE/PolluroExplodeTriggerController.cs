using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroExplodeTriggerController : TriggerController
{
    private const float DELAY_TIME = 4f;
    private float startTime = 0f;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private bool isBoom = false;

    // enable������start֮ǰִ�� ����awake���Ա�֤����ִ��
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

    // �̳в��ᴥ���������ظ�ʵ�ֵģ��������OnEnable������Ҳ�е���ʵ��ֻ���������
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

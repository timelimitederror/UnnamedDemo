using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroRockController : TriggerController
{
    private const float COOLDOWN_TIME = 10f;
    private const float RAY_COOLDOWN_TIME = 1f;
    private const float ATTACK_DISTANCE = 40f;
    private const float SPEED = 15f;

    private Action moveAction;
    private float cooldownTime = 0f;

    private Vector3 aim;
    private bool isMove = false;

    void Update()
    {
        transform.Rotate(Vector3.up, 100f * Time.deltaTime);
        if (isMove) // 移动
        {
            if (Vector3.Distance(transform.position, aim) <= 0.3f)
            {
                isMove = false;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, aim, SPEED * Time.deltaTime);
            return;
        }
        else if (Time.fixedTime >= cooldownTime) // 检查能不能发射，发射检查有间隔时间
        {
            cooldownTime = Time.fixedTime + RAY_COOLDOWN_TIME;
            Collider[] results = Physics.OverlapSphere(transform.position, ATTACK_DISTANCE, LayerMask.GetMask("Player"));
            if (results != null && results.Length > 0) // 如果攻击距离内有玩家，且rock和玩家连线路径上没有其他物体挡路，则可以攻击 小挡不算挡
            {
                Vector3 playerPosition = results[0].transform.position;
                playerPosition = new Vector3(playerPosition.x, playerPosition.y + 1.5f, playerPosition.z);
                Vector3 dir = (playerPosition - transform.position).normalized;
                RaycastHit[] hitInfos = Physics.RaycastAll(transform.position, dir, ATTACK_DISTANCE);
                if (hitInfos.Length > 0)
                {
                    foreach (RaycastHit hitInfo in hitInfos)
                    {
                        // Debug.Log(hitInfo.collider.gameObject.name);
                        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player")) // 发射
                        {
                            isMove = true;
                            aim = playerPosition;
                            cooldownTime = Time.fixedTime + COOLDOWN_TIME;
                            if (moveAction != null)
                            {
                                moveAction.Invoke();
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    void OnDisable()
    {
        isMove = false;
        cooldownTime = 0f;
    }

    public void SetMoveAction(Action moveAction)
    {
        this.moveAction = moveAction;
    }

    public void Release()
    {
        releaseAction.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkillBase
{
    // ʹ�ü��� ������˺���ִ���߼���������Ч��
    public abstract void UseSkill(PlayerStateController player);

    // ���ؼ��� ��������Ҫ�����ܳ�ʼ�����������Ϸ�����ϻ�ȡһЩ����һЩ�����������ЧԤ���岢Ԥװ����Ϸ��������
    public abstract void InstallSkill(EnemyControllerBase enemyController);

    // ж�ؼ��ܣ��ͷ���Դ
    public abstract void UninstallSkill();

    // ��ɫ״̬�Ƿ����㼼�ܷ�������
    public abstract bool Enable();
}

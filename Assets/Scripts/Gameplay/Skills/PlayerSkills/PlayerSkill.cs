using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill
{
    // ʹ�ü��ܣ�����˺���ִ���߼���������Ч��
    public abstract void useSkill();

    // ���ؼ��ܣ�������Ҫ�����ܳ�ʼ���������Player��Ϸ�����ϻ�ȡһЩ����һЩ�����������ЧԤ���岢Ԥװ�ڽ�ɫ����
    public abstract void installSkill(PlayerStateController playerController);

    // ж�ؼ��ܣ��ͷ���Դ
    public abstract void uninstallSkill();

    // ��ɫ״̬�Ƿ����㼼�ܷ�������
    public abstract bool enable();

    
}

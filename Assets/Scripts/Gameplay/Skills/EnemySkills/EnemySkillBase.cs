using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkillBase
{
    // 使用技能 ：造成伤害，执行逻辑，发动特效等
    public abstract void UseSkill(PlayerStateController player);

    // 加载技能 ，这里需要将技能初始化，比如从游戏对象上获取一些对象，一些组件，加载特效预设体并预装在游戏对象身上
    public abstract void InstallSkill(EnemyControllerBase enemyController);

    // 卸载技能，释放资源
    public abstract void UninstallSkill();

    // 角色状态是否满足技能发动条件
    public abstract bool Enable();
}

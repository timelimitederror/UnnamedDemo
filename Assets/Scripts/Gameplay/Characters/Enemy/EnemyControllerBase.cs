using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour
{
    protected EnemyType enemyType = EnemyType.Life;

    public abstract SkillColor getColor();

    public abstract void damage(SkillColor color, int value);

    public virtual EnemyType getEnemyType()
    {
        return enemyType;
    }

    public virtual EnemyControllerBase GetBody()
    {
        return this;
    }

    public enum EnemyType
    {
        Rock = 1,
        Life = 2
    }
}

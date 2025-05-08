using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour
{
    public abstract SkillColor getColor();

    public abstract void damage(SkillColor color, int value);

    public abstract EnemyType getEnemyType();

    public enum EnemyType { 
        Rock = 1,
        Life = 2
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroBodyController : EnemyControllerBase
{
    private PolluroController polluro;

    public void setPolluro(PolluroController polluro)
    {
        this.polluro = polluro;
    }

    public override void damage(SkillColor color, int value)
    {
        polluro.damage(color, value);
    }

    public override SkillColor getColor()
    {
        return polluro.getColor();
    }

    public override EnemyType getEnemyType()
    {
        return polluro.getEnemyType();
    }

    public override EnemyControllerBase GetBody()
    {
        return polluro;
    }
}

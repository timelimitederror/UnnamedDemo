using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillStateChanged
{
    public bool skill_1;
    public bool skill_2;
    public bool skill_3;
    public bool skill_4;
    public bool mixing;
    public bool normalAttack;

    public PlayerSkillStateChanged(bool skill_1, bool skill_2, bool skill_3, bool skill_4, bool mixing, bool normalAttack)
    {
        this.skill_1 = skill_1;
        this.skill_2 = skill_2;
        this.skill_3 = skill_3;
        this.skill_4 = skill_4;
        this.mixing = mixing;
        this.normalAttack = normalAttack;
    }
}

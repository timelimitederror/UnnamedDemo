using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillKeyPositionChanged
{
    public string skill_1;
    public string skill_2;
    public string skill_3;
    public string skill_4;
    public string mixing;
    public string armColor;

    public PlayerSkillKeyPositionChanged(
        string skill_1, string skill_2, string skill_3, string skill_4,
        string mixing, string armColor)
    {
        this.skill_1 = skill_1;
        this.skill_2 = skill_2;
        this.skill_3 = skill_3;
        this.skill_4 = skill_4;
        this.mixing = mixing;
        this.armColor = armColor;
    }
}

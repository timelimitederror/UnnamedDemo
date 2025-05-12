using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cube111Test : EnemyControllerBase
{
    public TextMeshPro tmp;
    private SkillColor thisColor = SkillColor.Red;
    private int redHealth = 1000;
    private int greenHealth = 1000;
    private int blueHealth = 1000;
    private EnemyType enemyType = EnemyType.Life;
    private Transform cameraTransform;

    void Start()
    {
        setText();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (thisColor == SkillColor.Red)
            {
                thisColor = SkillColor.Green;
            }
            else if (thisColor == SkillColor.Green)
            { thisColor = SkillColor.Blue; }
            else if (thisColor == SkillColor.Blue) { thisColor = SkillColor.Red; }
            setText();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (enemyType == EnemyType.Life)
            {
                enemyType = EnemyType.Rock;
            }
            else
            {
                enemyType = EnemyType.Life;
            }
            setText();
        }
        if(redHealth <=0 && greenHealth <= 0 && blueHealth <= 0)
        {
            die();
            redHealth = 1000;
            greenHealth = 1000;
            blueHealth = 1000;
            setText();
        }
        tmp.transform.forward = -cameraTransform.forward;
    }

    public override void damage(SkillColor color, int value)
    {
        switch (color)
        {
            case SkillColor.Red:
                redHealth -= value;
                break;
            case SkillColor.Green:
                greenHealth -= value;
                break;
            case SkillColor.Blue:
                blueHealth -= value;
                break;
            case SkillColor.Mixing:
                redHealth -= value;
                greenHealth -= value;
                blueHealth -= value;
                break;
            default:
                break;
        }
        setText();
    }

    private void setText()
    {
        tmp.SetText("red:" + redHealth + " green:" + greenHealth + " blue:" + blueHealth + " color:" + thisColor + " type:" + enemyType);
    }

    public override SkillColor getColor()
    {
        return thisColor;
    }

    public override EnemyType getEnemyType()
    {
        return enemyType;
    }

    private void die()
    {
        Debug.Log("µÐÈËËÀÁË");
    }
}

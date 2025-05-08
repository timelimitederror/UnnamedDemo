using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 控制和血量，颜料值，使用技能相关的功能
public class PlayerStateController : MonoBehaviour
{
    private const int SKILL_1 = 1;
    private const int SKILL_2 = 2;
    private const int SKILL_3 = 3;
    private const int SKILL_4 = 4;
    private const int MIXING = 5;
    private const int NORMAL_ATTACK = 6;

    //private PlayerController playerController;

    private KeyCode skill_1Key = KeyCode.Alpha1;
    private KeyCode skill_2Key = KeyCode.Alpha2;
    private KeyCode skill_3Key = KeyCode.Alpha3;
    private KeyCode skill_4Key = KeyCode.Alpha4;

    private KeyCode switchColorKey = KeyCode.R;
    private KeyCode mixingColorKey = KeyCode.F;
    private KeyCode normalAttackKey = KeyCode.Mouse0;

    public int health = 1000;
    public int mixingValue = 0;
    public int redValue = 95;
    public int greenValue = 95;
    public int blueValue = 95;

    public int maxHealth = 1000;
    public int maxMixing = 100;
    public int maxRed = 100;
    public int maxGreen = 100;
    public int maxBlue = 100;

    public float cooldownTime = 0f;
    public SkillColor color = SkillColor.Red;

    private bool skill_1Enable = false;
    private bool skill_2Enable = false;
    private bool skill_3Enable = false;
    private bool skill_4Enable = false;
    private bool mixingEnable = false;
    private bool normalAttackEnable = true;

    private float lastRestoreHealthTime = 0f;
    private int restoreHealthValue = 10;

    private Dictionary<int, PlayerSkill> skillDictionary = new Dictionary<int, PlayerSkill>();
    private List<ScheduleTask> scheduleTaskList = new List<ScheduleTask>();

    PlayerSpecialEffectController seController;

    void Start()
    {
        //playerController = GetComponent<PlayerController>();
        seController = GetComponent<PlayerSpecialEffectController>();

        // 加载技能
        skillDictionary[MIXING] = new MixingColor();
        skillDictionary[MIXING].installSkill(this);
        skillDictionary[NORMAL_ATTACK] = new NormalAttack();
        skillDictionary[NORMAL_ATTACK].installSkill(this);
        skillDictionary[SKILL_1] = new SingleAttackSkill01();
        skillDictionary[SKILL_1].installSkill(this);
        skillDictionary[SKILL_2] = new GroupAttackSkill01();
        skillDictionary[SKILL_2].installSkill(this);
        skillDictionary[SKILL_3] = new SingleAttackSkill02();
        skillDictionary[SKILL_3].installSkill(this);
        skillDictionary[SKILL_4] = new HealingSkill01();
        skillDictionary[SKILL_4].installSkill(this);

        // player UI 初始化
        setHealthUI();
        setSkillColorUI();
        setSkillStateUI();
        setColorValueUI();
    }

    void Update()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        ifDie();
        checkSkillEnable();
        useSkill();
        restoreHealth();
    }

    void FixedUpdate()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        runScheduleTask();
    }

    private void useSkill()
    {
        if (Input.GetKeyDown(switchColorKey))
        {
            switch (color)
            {
                case SkillColor.Red:
                    color = SkillColor.Green;
                    break;
                case SkillColor.Green:
                    color = SkillColor.Blue;
                    break;
                case SkillColor.Blue:
                    color = SkillColor.Red;
                    break;
                default:
                    break;
            }
            setSkillColorUI();
        }
        else if (Input.GetKeyDown(mixingColorKey))
        {
            if (mixingEnable)
            {
                skillDictionary[MIXING].useSkill();
            }
        }
        else if (Input.GetKeyDown(normalAttackKey))
        {
            if (normalAttackEnable)
            {
                skillDictionary[NORMAL_ATTACK].useSkill();
            }
        }
        else if (Input.GetKeyDown(skill_1Key))
        {
            if (skill_1Enable)
            {
                skillDictionary[SKILL_1].useSkill();
            }
        }
        else if (Input.GetKeyDown(skill_2Key))
        {
            if (skill_2Enable)
            {
                skillDictionary[SKILL_2].useSkill();
            }
        }
        else if (Input.GetKeyDown(skill_3Key))
        {
            if (skill_3Enable)
            {
                skillDictionary[SKILL_3].useSkill();
            }
        }
        else if (Input.GetKeyDown(skill_4Key))
        {
            if (skill_4Enable)
            {
                skillDictionary[SKILL_4].useSkill();
            }
        }
    }

    private void restoreHealth()
    {// 每秒自动恢复体力
        if (health < maxHealth && Time.fixedTime - lastRestoreHealthTime >= 1f)
        {
            lastRestoreHealthTime = Time.fixedTime;
            health += restoreHealthValue;
            health = health > maxHealth ? maxHealth : health;
            setHealthUI();
        }
    }

    public void hitSound()
    {
        seController.playHitSound();
    }

    private void ifDie()
    {
        if (health <= 0)
        {
            Debug.Log("你死了");
        }
    }

    // 加入定时任务（用来做dot或hot效果，或延迟执行的技能）
    public void addScheduleTask(ScheduleTask scheduleTask)
    {
        scheduleTaskList.Add(scheduleTask);
    }

    public void removeScheduleTaskByLobal(string lobal)
    {
        for (int i = 0; i < scheduleTaskList.Count; i++)
        {
            if (scheduleTaskList[i].lobal.Equals(lobal))
            {
                scheduleTaskList.RemoveAt(i);
                break;
            }
        }
    }

    private void runScheduleTask()
    {
        for (int i = 0; i < scheduleTaskList.Count; i++)
        {
            if (!scheduleTaskList[i].run())
            {
                scheduleTaskList.RemoveAt(i);
                i--;
            }
        }
    }

    // 检查所有技能是否可用
    private void checkSkillEnable()
    {
        skill_1Enable = skillDictionary[SKILL_1].enable();
        skill_2Enable = skillDictionary[SKILL_2].enable();
        skill_3Enable = skillDictionary[SKILL_3].enable();
        skill_4Enable = skillDictionary[SKILL_4].enable();
        mixingEnable = skillDictionary[MIXING].enable();
        normalAttackEnable = skillDictionary[NORMAL_ATTACK].enable();
        setSkillStateUI();
    }

    public void setHealthUI()
    {
        EventBus.Publish<PlayerHealthChanged>(new PlayerHealthChanged(health, maxHealth));
    }

    public void setColorValueUI()
    {
        EventBus.Publish<PlayerColorValueChanged>(new PlayerColorValueChanged(
            mixingValue, maxMixing,
            redValue, maxRed,
            greenValue, maxGreen,
            blueValue, maxBlue
            ));
    }

    private void setSkillColorUI()
    {
        EventBus.Publish<PlayerSkillColorChanged>(new PlayerSkillColorChanged(color));
    }

    private void setSkillStateUI()
    {
        EventBus.Publish<PlayerSkillStateChanged>(new PlayerSkillStateChanged(
            skill_1Enable, skill_2Enable, skill_3Enable, skill_4Enable,
            mixingEnable, normalAttackEnable
            ));
    }

    void OnDestroy()
    {
        foreach (PlayerSkill skill in skillDictionary.Values)
        {
            skill.uninstallSkill();
        }
    }

    public class ScheduleTask
    {
        public float lastUpdateTime;
        public float intervalTime;
        public int times;
        public Action action;
        public string lobal = "";

        public ScheduleTask(float intervalTime, int times, Action action)
        {
            this.lastUpdateTime = Time.fixedTime;
            this.intervalTime = intervalTime;
            this.times = times;
            this.action = action;
        }

        public ScheduleTask(float intervalTime, int times, Action action, string lobal)
        {
            this.lastUpdateTime = Time.fixedTime;
            this.intervalTime = intervalTime;
            this.times = times;
            this.action = action;
            this.lobal = lobal;
        }

        public bool run()
        {
            if (times <= 0)
            {
                return false;
            }
            if (Time.fixedTime - lastUpdateTime >= intervalTime)
            {
                lastUpdateTime = Time.fixedTime;
                times--;
                action.Invoke();
            }
            return true;
        }
    }

}

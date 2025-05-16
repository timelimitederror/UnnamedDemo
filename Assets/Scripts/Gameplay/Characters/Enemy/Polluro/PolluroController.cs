using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroController : EnemyControllerBase
{
    private const int SKILL_1 = 1; // 改变自身颜色
    private const int SKILL_2 = 2; // 平a
    private const int SKILL_3 = 3; // 创造小岩石
    private const int SKILL_4 = 4; // 爆炸区
    private const float FIGHTING_DISTANCE = 50f;

    private PolluroAnimationController animatorController;

    private List<ModelColorController> modelColorControllers = new List<ModelColorController>();
    private BehaviorTreeBuilder behaviorTreeBuilder = new BehaviorTreeBuilder();

    public SkillColor color = SkillColor.Red;
    public int redHealth = 1000;
    public int greenHealth = 1000;
    public int blueHealth = 1000;

    public int maxRed = 1000;
    public int maxGreen = 1000;
    public int maxBlue = 1000;

    public float cooldownTime = 0f;
    private float explodeCooldownTime = 0f;
    private float rockCooldownTime = 0f;

    private PlayerStateController playerStateController;
    private List<ScheduleTask> scheduleTaskList = new List<ScheduleTask>();
    private Dictionary<int, EnemySkillBase> skillDictionary = new Dictionary<int, EnemySkillBase>();
    private bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        CheckBody(transform);
        animatorController = GetComponent<PolluroAnimationController>();

        // 加载技能
        skillDictionary[SKILL_1] = new PolluroChangeColorSkill();
        skillDictionary[SKILL_1].InstallSkill(this);
        skillDictionary[SKILL_2] = new PolluroNormalAttack();
        skillDictionary[SKILL_2].InstallSkill(this);
        skillDictionary[SKILL_3] = new PolluroGenerateRockSkill();
        skillDictionary[SKILL_3].InstallSkill(this);
        skillDictionary[SKILL_4] = new PolluroExplodeSkill();
        skillDictionary[SKILL_4].InstallSkill(this);

        isDie = false;
        BuildBehaviorTree();
        SetPolluroRGBHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }

        // 这个要删掉=========================
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    behaviorTreeBuilder.DebugTree();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    skillDictionary[SKILL_1].UseSkill(playerStateController);
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    skillDictionary[SKILL_2].UseSkill(playerStateController);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    skillDictionary[SKILL_3].UseSkill(playerStateController);
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    skillDictionary[SKILL_4].UseSkill(playerStateController);
        //}
        // =====================================

        if (playerStateController == null)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, FIGHTING_DISTANCE, LayerMask.GetMask("Player"));
            if (results != null && results.Length > 0)
            {
                playerStateController = results[0].gameObject.GetComponent<PlayerStateController>();
            }
        }
        else if (!isDie)
        {
            if (redHealth <= 0 && greenHealth <= 0 && blueHealth <= 0)
            {
                isDie = true;
                Die();
                return;
            }
            if (playerStateController.IsDie)
            {
                return;
            }
            behaviorTreeBuilder.TreeTick();
        }
    }

    void FixedUpdate()
    {
        if (!TimeManager.Instance.timeFlow())
        {
            return;
        }
        RunScheduleTask();
    }

    void OnDestroy()
    {
        foreach (EnemySkillBase skill in skillDictionary.Values)
        {
            skill.UninstallSkill();
        }
        skillDictionary.Clear();
    }

    public override void damage(SkillColor color, int value)
    {
        switch (color)
        {
            case SkillColor.Red:
                redHealth -= value;
                redHealth = redHealth < 0 ? 0 : redHealth;
                break;
            case SkillColor.Green:
                greenHealth -= value;
                greenHealth = greenHealth < 0 ? 0 : greenHealth;
                break;
            case SkillColor.Blue:
                blueHealth -= value;
                blueHealth = blueHealth < 0 ? 0 : blueHealth;
                break;
            case SkillColor.Mixing:
                redHealth -= value;
                redHealth = redHealth < 0 ? 0 : redHealth;
                greenHealth -= value;
                greenHealth = greenHealth < 0 ? 0 : greenHealth;
                blueHealth -= value;
                blueHealth = blueHealth < 0 ? 0 : blueHealth;
                break;
            default:
                break;
        }
        SetPolluroRGBHealthUI();
    }

    public override SkillColor getColor()
    {
        return color;
    }

    private void CheckBody(Transform transform)
    {
        Collider collider = transform.GetComponent<Collider>();
        if (collider != null)
        {
            transform.gameObject.AddComponent<PolluroBodyController>().setPolluro(this);
        }
        ModelColorController modelColorController = transform.gameObject.GetComponent<ModelColorController>();
        if (modelColorController != null)
        {
            modelColorControllers.Add(modelColorController);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            CheckBody(transform.GetChild(i));
        }
    }

    private void SetPolluroRGBHealthUI()
    {
        EventBus.Publish(new PolluroRGBHealthChanged(
            redHealth, maxRed,
            greenHealth, maxGreen,
            blueHealth, maxBlue
            ));
    }

    public float GetDistance()
    {
        Vector3 playerPosition = playerStateController.transform.position;
        playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
        Vector3 polluroPosition = new Vector3(transform.position.x, 0, transform.position.z);
        return Vector3.Distance(playerPosition, polluroPosition);
    }

    public void ChangeColor(SkillColor newColor)
    {
        this.color = newColor;
        foreach (var controller in modelColorControllers)
        {
            controller.SetColor(newColor);
        }
    }

    private void Die()
    {
        animatorController.Die();
        // Debug.Log("Polluro死了");
        foreach (EnemySkillBase skill in skillDictionary.Values)
        {
            skill.UninstallSkill();
        }
        skillDictionary.Clear();
        EventBus.Publish(new GameScene01Victory());
    }

    private void BuildBehaviorTree()
    {
        // 开始开始
        // 变换颜色使用条件：必须满足enable 当前血量与最高血量差>5%*max ，如果当前血量归零，立即变换颜色 这个不受冷却影响
        // 创造小岩石使用条件：必须满足enable 检测玩家和boss的距离<35f 内部冷却10s 这个受公共冷却和私有冷却影响
        // 爆炸使用条件：必须满足enable 检测玩家和boss的距离<40f 内部冷却20s 这个受公共冷却和私有冷却影响
        // 平A使用条件： 必须满足enable 检测玩家和boss的距离<50f 填充技能 这个受公共冷却影响
        /*
         Selector
            Filter
                [SimpleLeaf: if ChangeColor enable SKILL_1
                [Filter
                    [Selector
                        [SimpleLeaf: if 当前血量归零
                        [SimpleLeaf: if 当前血量与最高血量差>10%*max
                SimpleLeaf: use ChangeColor SKILL_1
            Filter
                SimpleLeaf: if cooldownTime <= Time.fixedTime 公共冷却结束
                Selector
                    Filter
                        [SimpleLeaf: if rockCooldownTime <= Time.fixedTime 私有冷却结束
                        [SimpleLeaf: if distance < 35f
                        [SimpleLeaf: if Rock enable SKILL_3
                        SimpleLeaf: use Rock SKILL_3 rockCooldownTime+=10f
                    Filter
                        [SimpleLeaf: if explodeCooldownTime <= Time.fixedTime 私有冷却结束
                        [SimpleLeaf: if distance < 40f
                        [SimpleLeaf: if Rock enable SKILL_4
                        SimpleLeaf: use Rock SKILL_4 explodeCooldownTime+=20f
                    Filter
                        [SimpleLeaf: if NA enable SKILL_2
                        [SimpleLeaf: if distance < 50f
                        SimpleLeaf: use NA SKILL_2
         */
        behaviorTreeBuilder.Seletctor("")
                                .Filter("变色技能")
                                    .SimpleLeaf("变色技能使用条件", () =>
                                    {
                                        if (!skillDictionary[SKILL_1].Enable())
                                        {
                                            return Node.Status.Failure;
                                        }
                                        switch (color)
                                        {
                                            case SkillColor.Red:
                                                if (redHealth <= 0
                                                || greenHealth - redHealth >= maxRed / 20
                                                || blueHealth - redHealth >= maxRed / 20)
                                                {
                                                    return Node.Status.Success;
                                                }
                                                break;
                                            case SkillColor.Green:
                                                if (greenHealth <= 0
                                                || redHealth - greenHealth >= maxGreen / 20
                                                || blueHealth - greenHealth >= maxGreen / 20)
                                                {
                                                    return Node.Status.Success;
                                                }
                                                break;
                                            case SkillColor.Blue:
                                                if (blueHealth <= 0
                                                || redHealth - blueHealth >= maxBlue / 20
                                                || greenHealth - blueHealth >= maxBlue / 20)
                                                {
                                                    return Node.Status.Success;
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                        return Node.Status.Failure;
                                    })
                                    .SimpleLeaf("使用变色技能", () =>
                                    {
                                        skillDictionary[SKILL_1].UseSkill(playerStateController);
                                        return Node.Status.Success;
                                    })
                                    .Back()
                                .Filter("其他技能")
                                    .SimpleLeaf("公共冷却", () =>
                                    {
                                        if (cooldownTime <= Time.fixedTime)
                                        {
                                            return Node.Status.Success;
                                        }
                                        return Node.Status.Failure;
                                    })
                                    .Seletctor("")
                                        .Filter("岩石技能")
                                            .SimpleLeaf("岩石技能使用条件", () =>
                                            {
                                                if (rockCooldownTime > Time.fixedTime
                                                || Vector3.Distance(transform.position, playerStateController.transform.position) > 40f
                                                || !skillDictionary[SKILL_3].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("使用岩石技能", () =>
                                            {
                                                skillDictionary[SKILL_3].UseSkill(playerStateController);
                                                rockCooldownTime = Time.fixedTime + 10f;
                                                return Node.Status.Success;
                                            })
                                            .Back()
                                        .Filter("爆炸技能")
                                            .SimpleLeaf("爆炸技能使用条件", () =>
                                            {
                                                if (explodeCooldownTime > Time.fixedTime
                                                || Vector3.Distance(transform.position, playerStateController.transform.position) > 40f
                                                || !skillDictionary[SKILL_4].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("使用爆炸技能", () =>
                                            {
                                                skillDictionary[SKILL_4].UseSkill(playerStateController);
                                                explodeCooldownTime = Time.fixedTime + 20f;
                                                return Node.Status.Success;
                                            })
                                            .Back()
                                        .Filter("平A")
                                            .SimpleLeaf("平A使用条件", () =>
                                            {
                                                if (Vector3.Distance(transform.position, playerStateController.transform.position) > 50f
                                                || !skillDictionary[SKILL_2].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("使用平A", () =>
                                            {
                                                skillDictionary[SKILL_2].UseSkill(playerStateController);
                                                return Node.Status.Success;
                                            })
                            .End();
    }

    // 加入定时任务（用来做dot或hot效果，或延迟执行的技能）
    public void AddScheduleTask(ScheduleTask scheduleTask)
    {
        scheduleTaskList.Add(scheduleTask);
    }

    public void RemoveScheduleTaskByLobal(string lobal)
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

    private void RunScheduleTask()
    {
        for (int i = 0; i < scheduleTaskList.Count; i++)
        {
            if (!scheduleTaskList[i].Run())
            {
                scheduleTaskList.RemoveAt(i);
                i--;
            }
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

        public bool Run()
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

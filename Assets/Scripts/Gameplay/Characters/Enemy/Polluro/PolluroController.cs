using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroController : EnemyControllerBase
{
    private const int SKILL_1 = 1; // �ı�������ɫ
    private const int SKILL_2 = 2; // ƽa
    private const int SKILL_3 = 3; // ����С��ʯ
    private const int SKILL_4 = 4; // ��ը��
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

        // ���ؼ���
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

        // ���Ҫɾ��=========================
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
        // Debug.Log("Polluro����");
        foreach (EnemySkillBase skill in skillDictionary.Values)
        {
            skill.UninstallSkill();
        }
        skillDictionary.Clear();
        EventBus.Publish(new GameScene01Victory());
    }

    private void BuildBehaviorTree()
    {
        // ��ʼ��ʼ
        // �任��ɫʹ����������������enable ��ǰѪ�������Ѫ����>5%*max �������ǰѪ�����㣬�����任��ɫ ���������ȴӰ��
        // ����С��ʯʹ����������������enable �����Һ�boss�ľ���<35f �ڲ���ȴ10s ����ܹ�����ȴ��˽����ȴӰ��
        // ��ըʹ����������������enable �����Һ�boss�ľ���<40f �ڲ���ȴ20s ����ܹ�����ȴ��˽����ȴӰ��
        // ƽAʹ�������� ��������enable �����Һ�boss�ľ���<50f ��似�� ����ܹ�����ȴӰ��
        /*
         Selector
            Filter
                [SimpleLeaf: if ChangeColor enable SKILL_1
                [Filter
                    [Selector
                        [SimpleLeaf: if ��ǰѪ������
                        [SimpleLeaf: if ��ǰѪ�������Ѫ����>10%*max
                SimpleLeaf: use ChangeColor SKILL_1
            Filter
                SimpleLeaf: if cooldownTime <= Time.fixedTime ������ȴ����
                Selector
                    Filter
                        [SimpleLeaf: if rockCooldownTime <= Time.fixedTime ˽����ȴ����
                        [SimpleLeaf: if distance < 35f
                        [SimpleLeaf: if Rock enable SKILL_3
                        SimpleLeaf: use Rock SKILL_3 rockCooldownTime+=10f
                    Filter
                        [SimpleLeaf: if explodeCooldownTime <= Time.fixedTime ˽����ȴ����
                        [SimpleLeaf: if distance < 40f
                        [SimpleLeaf: if Rock enable SKILL_4
                        SimpleLeaf: use Rock SKILL_4 explodeCooldownTime+=20f
                    Filter
                        [SimpleLeaf: if NA enable SKILL_2
                        [SimpleLeaf: if distance < 50f
                        SimpleLeaf: use NA SKILL_2
         */
        behaviorTreeBuilder.Seletctor("")
                                .Filter("��ɫ����")
                                    .SimpleLeaf("��ɫ����ʹ������", () =>
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
                                    .SimpleLeaf("ʹ�ñ�ɫ����", () =>
                                    {
                                        skillDictionary[SKILL_1].UseSkill(playerStateController);
                                        return Node.Status.Success;
                                    })
                                    .Back()
                                .Filter("��������")
                                    .SimpleLeaf("������ȴ", () =>
                                    {
                                        if (cooldownTime <= Time.fixedTime)
                                        {
                                            return Node.Status.Success;
                                        }
                                        return Node.Status.Failure;
                                    })
                                    .Seletctor("")
                                        .Filter("��ʯ����")
                                            .SimpleLeaf("��ʯ����ʹ������", () =>
                                            {
                                                if (rockCooldownTime > Time.fixedTime
                                                || Vector3.Distance(transform.position, playerStateController.transform.position) > 40f
                                                || !skillDictionary[SKILL_3].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("ʹ����ʯ����", () =>
                                            {
                                                skillDictionary[SKILL_3].UseSkill(playerStateController);
                                                rockCooldownTime = Time.fixedTime + 10f;
                                                return Node.Status.Success;
                                            })
                                            .Back()
                                        .Filter("��ը����")
                                            .SimpleLeaf("��ը����ʹ������", () =>
                                            {
                                                if (explodeCooldownTime > Time.fixedTime
                                                || Vector3.Distance(transform.position, playerStateController.transform.position) > 40f
                                                || !skillDictionary[SKILL_4].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("ʹ�ñ�ը����", () =>
                                            {
                                                skillDictionary[SKILL_4].UseSkill(playerStateController);
                                                explodeCooldownTime = Time.fixedTime + 20f;
                                                return Node.Status.Success;
                                            })
                                            .Back()
                                        .Filter("ƽA")
                                            .SimpleLeaf("ƽAʹ������", () =>
                                            {
                                                if (Vector3.Distance(transform.position, playerStateController.transform.position) > 50f
                                                || !skillDictionary[SKILL_2].Enable())
                                                {
                                                    return Node.Status.Failure;
                                                }
                                                return Node.Status.Success;
                                            })
                                            .SimpleLeaf("ʹ��ƽA", () =>
                                            {
                                                skillDictionary[SKILL_2].UseSkill(playerStateController);
                                                return Node.Status.Success;
                                            })
                            .End();
    }

    // ���붨ʱ����������dot��hotЧ�������ӳ�ִ�еļ��ܣ�
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

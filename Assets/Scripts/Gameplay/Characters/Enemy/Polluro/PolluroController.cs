using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroController : EnemyControllerBase
{
    private const int SKILL_1 = 1;
    private const int SKILL_2 = 2;
    private const int SKILL_3 = 3;
    private const int SKILL_4 = 4;
    private const int SKILL_5 = 5;
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

    private PlayerStateController playerStateController;
    private List<ScheduleTask> scheduleTaskList = new List<ScheduleTask>();
    private Dictionary<int, EnemySkillBase> skillDictionary = new Dictionary<int, EnemySkillBase>();
    private List<EnemySkillBase> skillQueue = new List<EnemySkillBase>();

    // 岩石小怪
    public List<GameObject> rockEnemy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CheckBody(transform);
        GetBaseModelColorController();
        animatorController = GetComponent<PolluroAnimationController>();

        // 加载技能
        //skillDictionary[SKILL_1] = new SingleAttackSkill01();
        //skillDictionary[SKILL_1].installSkill(this);
        //skillDictionary[SKILL_2] = new GroupAttackSkill01();
        //skillDictionary[SKILL_2].installSkill(this);
        //skillDictionary[SKILL_3] = new SingleAttackSkill02();
        //skillDictionary[SKILL_3].installSkill(this);
        //skillDictionary[SKILL_4] = new HealingSkill01();
        //skillDictionary[SKILL_4].installSkill(this);
        //skillDictionary[SKILL_5] = new HealingSkill01();
        //skillDictionary[SKILL_5].installSkill(this);


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

        if (Input.GetKeyDown(KeyCode.L))
        {
            behaviorTreeBuilder.DebugTree();
        }
        if (playerStateController == null)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, FIGHTING_DISTANCE, LayerMask.GetMask("Player"));
            if (results != null && results.Length > 0)
            {
                playerStateController = results[0].gameObject.GetComponent<PlayerStateController>();
            }
        }
        else
        {
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

    public override EnemyType getEnemyType()
    {
        return EnemyType.Life;
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

    private void GetBaseModelColorController()
    {
        ModelColorController controller = null;
        controller = transform.parent.Find("Base")?.gameObject.GetComponent<ModelColorController>();
        if (controller != null)
        {
            modelColorControllers.Add(controller);
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

    //public void ChangeColorByHealth()
    //{
    //    if (redHealth >= greenHealth && redHealth >= blueHealth)
    //    {
    //        ChangeColor(SkillColor.Red);
    //    }
    //    else if (greenHealth >= blueHealth)
    //    {
    //        ChangeColor(SkillColor.Green);
    //    }
    //    else
    //    {
    //        ChangeColor(SkillColor.Blue);
    //    }
    //}

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
        Debug.Log("Polluro死了");
    }

    private void BuildBehaviorTree()
    {
        
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

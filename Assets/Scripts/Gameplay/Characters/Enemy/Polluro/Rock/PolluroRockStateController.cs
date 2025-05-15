using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroRockStateController : EnemyControllerBase
{
    public int colorInt = 0;

    private int health = 100;
    private int maxHealth = 100;
    private SkillColor color;
    private PolluroRockController rockController;

    public override void damage(SkillColor color, int value)
    {
        health -= value;
        health = health < 0 ? 0 : health;
        SetHealth();
    }

    public override SkillColor getColor()
    {
        return color;
    }

    void OnEnable()
    {
        health = maxHealth;
        EventBus.Publish(new PolluroRockAddUI(gameObject));
    }

    void Start()
    {
        enemyType = EnemyType.Rock;
        color = (SkillColor)colorInt;
        rockController = GetComponent<PolluroRockController>();
    }

    void Update()
    {
        if (health <= 0)
        {
            rockController.Release();
        }
        SetHealth();
    }

    void OnDisable()
    {
        EventBus.Publish(new PolluroRockDestroy(gameObject));
    }

    void OnDestroy()
    {
        EventBus.Publish(new PolluroRockDestroy(gameObject));
    }

    private void SetHealth()
    {
        EventBus.Publish(new PolluroRockHealthChanged(gameObject, health, maxHealth, transform.position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroRockHealthChanged
{
    public GameObject gameObject;
    public int health;
    public int maxHealth;
    public Vector3 position;

    public PolluroRockHealthChanged(GameObject gameObject, int health, int maxHealth, Vector3 position)
    {
        this.gameObject = gameObject;
        this.health = health;
        this.maxHealth = maxHealth;
        this.position = position;
    }
}

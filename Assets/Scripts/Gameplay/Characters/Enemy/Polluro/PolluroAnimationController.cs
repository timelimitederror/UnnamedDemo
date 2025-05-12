using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluroAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack01()
    {
        animator.SetTrigger("attack01");
    }

    public void Attack02()
    {
        animator.SetTrigger("attack02");
    }

    public void Attack03()
    {
        animator.SetTrigger("attack03");
    }

    public void Die()
    {
        animator.SetBool("die", true);
    }
}

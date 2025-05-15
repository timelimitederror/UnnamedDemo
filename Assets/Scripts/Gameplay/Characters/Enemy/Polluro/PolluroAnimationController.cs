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

    public void Attack01() // Í¶ÖÀ
    {
        animator.SetTrigger("attack01");
    }

    public void Attack02() // ×óÊÖ´Á
    {
        animator.SetTrigger("attack02");
    }

    public void Attack03() // ºð½Ð
    {
        animator.SetTrigger("attack03");
    }

    public void Die()
    {
        animator.SetBool("die", true);
    }
}

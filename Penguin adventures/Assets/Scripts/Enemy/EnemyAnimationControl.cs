using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationControl : MonoBehaviour
{
    protected Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): GetComponent<Animator>() not found");
    }

    public void Walke() => animator.Play("Walke");

    public void Sleep() => animator.Play("Sleep");

    public void Look() => animator.Play("Look");

    public void Attack() => animator.Play("Attack");
}

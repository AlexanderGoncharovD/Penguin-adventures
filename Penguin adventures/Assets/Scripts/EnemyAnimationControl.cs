using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationControl : MonoBehaviour
{
    private Animator animator;
    private Transform characterCenter,
        mainCamera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterCenter = transform.FindChild("center").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Start()
    {
        
    }

    public void StateAnimation(float direction)
    {
        
            if (direction > 0)
            {
                characterCenter.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                characterCenter.localScale = new Vector3(1, 1, 1);
            }
    }

    public void Walke()
    {
        animator.Play("Walke");
    }

    public void Sleep()
    {
        animator.Play("Sleep");
    }

    public void Look()
    {
        animator.Play("Look");
    }

    public void Attack()
    {
        animator.Play("Attack");
    }
}

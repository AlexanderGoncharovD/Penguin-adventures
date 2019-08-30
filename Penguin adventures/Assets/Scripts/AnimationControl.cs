using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    
    private Animator animator;
    private Transform characterCenter,
        mainCamera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterCenter = transform.FindChild("center").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void StateAnimation(float direction)
    {
        if (direction == 0)
        {
            animator.SetBool("Walk", false); // Закончить анимацию ходьбы
        }
        else
        {
            animator.SetBool("Walk", true); // Начать анимацию ходьбы
            if (direction > 0)
            {
                characterCenter.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                characterCenter.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void Jump(bool isJump)
    {
        if (isJump)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    public void EatFish()
    {
        animator.SetBool("Walk", false); // Закончить анимацию ходьбы
        animator.SetBool("Jump", false);
        animator.Play("Eat");
    }

    public void Damage(string name)
    {
        animator.Play("Snake damage");
    }

    public void Death()
    {
        animator.Play("Death");
    }
}

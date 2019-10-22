using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    protected Animator anim;
    protected float flip = 0f;
    private Transform _center;

    private void Awake()
    {
        _center = transform.Find("center");
        if (_center == null)
            Debug.Log("(Animations.cs): " + gameObject.name + " 'center' not found");
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("(Animations.cs): " + gameObject.name + " Component<Animator> not found");
    }

    public virtual void Update()
    {
        if (flip > 0)
            _center.localScale = new Vector3(-1, 1, 1);
        else if (flip < 0)
            _center.localScale = Vector3.one;
    }

    public virtual void Walk()
    {
        if (flip != 0)
        {
            if (anim.GetBool("walk") == false)
                anim.SetBool("walk", true);
        }
        else
        {
            if (anim.GetBool("walk") == true)
                anim.SetBool("walk", false);
        }
    }

    public virtual void Jump(int countJump)
    {
        switch (countJump)
        {
            case 1:
                anim.SetBool("jump", true);
                anim.SetBool("walk", false);
                break;
            case 2:
                anim.CrossFade("Jump", 0.25f, 0, 0);
                break;
        }
    }

    public virtual void JumpLanded()
    {
        anim.SetBool("jump", false);
    }

    public virtual void Eat()
    {
        anim.Play("Eat");
    }

    public virtual void Idle()
    {
        anim.SetBool("walk", false);
        anim.SetBool("jump", false);
        anim.Play("Idle");
    }

    public virtual void Died()
    {
        anim.SetBool("walk", false);
        anim.SetBool("jump", false);
        anim.Play("Death");
        GameLevel.IsCharacterDied = true;

    }
}

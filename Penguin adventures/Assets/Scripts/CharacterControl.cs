using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{

    public float Speed,
        JumpForce,
        jumpReductionIndex = 2.0f,
        SlowDown = 5.0f;
    public FixedJoystick Joystick;

    private float curJumpForce;
    private int curNumberJumpClick;
    private bool isJump,
        isEat,
        isMove,
        isDeath;
    private Rigidbody rigidbody;
    private AnimationControl animation;
    private CapsuleCollider collider;
    private Vector3 contactPoint;
    private Global global;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animation = GetComponent<AnimationControl>();
        collider = GetComponent<CapsuleCollider>();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
    }

    private void Start()
    {
        curJumpForce = JumpForce;
        isMove = true;
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            Vector3 direction = transform.forward * Joystick.Vertical + transform.right * Joystick.Horizontal;
            rigidbody.velocity = new Vector3(direction.x * Speed, rigidbody.velocity.y, direction.z * Speed);
        }

        if (!isDeath)
        {
            // Изменение анимации в зависимости от положения джойстика
            animation.StateAnimation(Joystick.Horizontal);
        }

    }

    private void Update()
    {
        // Уменьшение сопротивления при падении
        if (rigidbody.drag > 0)
        {
            rigidbody.drag -= SlowDown * 2 * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var tag = other.tag;
        if (tag != "Star" && tag != "Fish" && tag != "Enemy")
        {
            if (isJump)
            {
                JumpEnd();
            }
        }
    }

    public void Jump()
    {
        if (isMove)
        {
            if (curNumberJumpClick < 1)
            {
                isJump = true;
                //rigidbody.AddForce(transform.up * curJumpForce, ForceMode.Impulse);

                rigidbody.velocity = new Vector3(0, curJumpForce, 0);
                curJumpForce /= jumpReductionIndex;
                curNumberJumpClick++;
                animation.Jump(isJump);
            }
            else
            {
                rigidbody.drag = SlowDown;
            }
        }
    }

    private void JumpEnd()
    {
        isJump = false;
        animation.Jump(isJump);
        curJumpForce = JumpForce;
        curNumberJumpClick = 0;
    }

    public void EatFish()
    {
        isEat = true;
        isMove = false;
        animation.EatFish();
        StopMotion();
    }

    public void FinishedEatingFish()
    {
        global.IncreaseHeart(1);
        isEat = false;
        isMove = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
            // Оттолкнуть персонажа от стены, если он к ней прижался и пытается прыгнуть
            if (isJump && contact.point.y > transform.position.y + 0.1f)
            {
                rigidbody.drag = 0;
            }
        }
    }

    private void StopMotion() => rigidbody.velocity = Vector3.zero;

    public int Damage(int damage, string name)
    {
        animation.Damage(name);
        StopMotion();
        var curHeart = global.ReduceHeart(damage);
        if (curHeart <= 0)
        {
            animation.Death();
            isMove = false;
            StopMotion();
            isDeath = true;
        }
        return curHeart;
    }
}

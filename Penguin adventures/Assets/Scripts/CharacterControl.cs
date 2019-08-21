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
        isEat;
    private Rigidbody rigidbody;
    private AnimationControl animation;
    private CapsuleCollider collider;
    private Vector3 contactPoint;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animation = GetComponent<AnimationControl>();
        collider = GetComponent<CapsuleCollider>();
        curJumpForce = JumpForce;
    }

    private void FixedUpdate()
    {
        if (!isEat)
        {
            Vector3 direction = transform.forward * Joystick.Vertical + transform.right * Joystick.Horizontal;
            rigidbody.velocity = new Vector3(direction.x * Speed, rigidbody.velocity.y, direction.z * Speed);

        }
        // Изменение анимации в зависимости от положения джойстика
        animation.StateAnimation(Joystick.Horizontal);
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
        if (other.tag != "Star" && other.tag != "Fish")
        {
            if (isJump)
            {
                JumpEnd();
            }
        }
    }

    public void Jump()
    {
        if (!isEat)
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
        animation.EatFish();
        rigidbody.velocity = Vector3.zero;
    }

    public void FinishedEatingFish()
    {
        isEat = false;
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
}

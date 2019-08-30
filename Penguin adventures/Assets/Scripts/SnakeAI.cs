using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeAction
{
    Patrol,
    Guard,
}

public enum SnakeAction
{
    Sleep,
    Look,
    Walke,
    Attack,
}

public class SnakeAI : MonoBehaviour
{
    public TypeAction Behavior;
    public SnakeAction Action;
    public Transform Area;
    [Range(0.0f, 1.0f)]
    public float PercentOfCharacterSpeed = 0.9f;
    public float DistDetected = 2.0f,
        DistPursuit = 5.0f,
        TimerAttak = 2.0f,
        DistAttack = 1.5f,
        TimerResumeAttack = 2.0f;
    public int Damage = 1;

    private Transform character,
        target,
        mainCamera;
    private Rigidbody rigidbody;
    private float speed,
        colliderHeight;
    private Vector3 thisPos,
        targetPos,
        areaPos,
        areaScale;
    private EnemyAnimationControl animation;
    private Camera camera;
    private bool isBite;
    private SphereCollider sphereCollider;
    private CapsuleCollider collider;

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rigidbody = GetComponent<Rigidbody>();
        animation = GetComponent<EnemyAnimationControl>();
        camera = mainCamera.GetComponent<Camera>();
        sphereCollider = GetComponent<SphereCollider>();
        collider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        speed = character.GetComponent<CharacterControl>().Speed * PercentOfCharacterSpeed;
        colliderHeight = collider.height - 0.05f;

        areaPos = Area.position;
        areaScale = Area.localScale;

        if (Behavior == TypeAction.Patrol)
        {
            if (target == null)
            {
                GenerationTargetPoint();
            }
        }
        else if (Behavior == TypeAction.Guard)
        {
            Sleep();
        }
    }

    // Update is called once per frame
    void Update()
    {
        thisPos = transform.position;

        if (Action == SnakeAction.Sleep)
        {

        }
        else
        {
            if (target == null)
            {
                if (Behavior == TypeAction.Patrol)
                {
                    GenerationTargetPoint();
                }
            }
            else
            {
                targetPos = target.position;

                if (!isBite)
                {
                    if (Action == SnakeAction.Walke || Action == SnakeAction.Attack)
                    {
                        if (Vector3.Distance(thisPos, targetPos) > 1.5f)
                        {
                            Vector3 normalized = (targetPos - thisPos).normalized;
                            Vector3 direction = transform.forward * normalized.z + transform.right * normalized.x;
                            rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);

                            animation.StateAnimation((camera.WorldToScreenPoint(targetPos) - camera.WorldToScreenPoint(transform.position)).x);
                        }
                        else
                        {
                            if (Action == SnakeAction.Attack)
                            {
                                rigidbody.isKinematic = true;
                                animation.Attack();
                                isBite = true;
                            }
                            else
                            {

                                Destroy(target.gameObject);
                            }
                            StopMotion();
                        }
                    }
                }
            }
        }
    }

    private void GenerationTargetPoint()
    {
        target = new GameObject().transform;
        target.transform.position = new Vector3(Random.Range(areaPos.x - areaScale.x / 2, areaPos.x + areaScale.x / 2),
            thisPos.y, Random.Range(areaPos.z - areaScale.z / 2, areaPos.z + areaScale.z / 2));
        
    }

    private void StopMotion() => rigidbody.velocity = Vector3.zero;

    private void Sleep()
    {
        animation.Sleep();
        StopMotion();
        sphereCollider.radius = DistDetected;
        target = null;
        Action = SnakeAction.Sleep;
        rigidbody.isKinematic = true;
    }

    private void Look()
    {
        Action = SnakeAction.Look;
        animation.StateAnimation((camera.WorldToScreenPoint(character.position) - camera.WorldToScreenPoint(transform.position)).x);
        animation.Look();
        sphereCollider.radius = DistPursuit;
        StartCoroutine(Attack(TimerAttak));
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Behavior == TypeAction.Guard)
            {
                if (Action != SnakeAction.Look)
                {
                    Look();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Sleep();
        }
    }

    private IEnumerator Attack(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (Action == SnakeAction.Look)
        {
            rigidbody.isKinematic = false;
            Action = SnakeAction.Attack;
            target = character;
            animation.Walke();
        }
    }

    private void Bite()
    {
        
        var curCharacterHeart = target.GetComponent<CharacterControl>().Damage(Damage, "Snake");
        if (curCharacterHeart <= 0)
        {
            Sleep();
            GetComponent<SnakeAI>().enabled = false;
        }
    }

    public void FinishedAttack()
    {
        rigidbody.isKinematic = false;
        target = character;
        if (Vector3.Distance(thisPos, targetPos) > 1.5f && Action != SnakeAction.Sleep)
        {
            animation.Walke();
        }
       isBite = false;
    }

    public IEnumerator ResumeAttack()
    {
        yield return new WaitForSeconds(TimerResumeAttack);
        OnTriggerEnter(character.GetComponent<Collider>());
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
            // Оттолкнуть персонажа от стены, если он к ней прижался и пытается прыгнуть
            if (contact.point.y > transform.position.y + colliderHeight)
            {
                if (Action == SnakeAction.Sleep)
                {
                    //collision.rigidbody.velocity = (contact.point - collision.transform.position).normalized * 2;
                }
                else
                {
                    StartCoroutine(ResumeAttack());
                    Sleep();
                }
            }
        }
    }
}

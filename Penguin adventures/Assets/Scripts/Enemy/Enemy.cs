using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyAnimationControl
{
    public int Health, Damage;
    public float SpeedMove = 2.0f, TimeBeforAttack = 3.0f, DistanceAttack = 1.5f, DistanceDamage = 0.5f;
    public Vector2 RadiusTrigger = new Vector2(2.5f, 5.0f);
    public AudioClip Audio_Attack;

    private Transform character, center;
    protected Transform target;
    protected E_Action action;
    private float timer;
    private bool isTimerActive;
    private new Camera camera;
    protected new AudioSource audio;
    private SphereCollider triggerCollider;
    protected Vector3 targetPos, thisPos, normalizeMove, directionMove;
    protected Rigidbody rigidbody;

    public override void Start()
    {
        base.Start();
        character = GameObject.FindGameObjectWithTag("Player").transform;
        if (character == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Player not found");
        center = gameObject.transform.Find("center").transform;
        if (center == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Center not found");
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (camera == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Component<Camera> not found");
        audio = GetComponent<AudioSource>();
        if (audio == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Component<AudioSource> not found");
        triggerCollider = GetComponent<SphereCollider>();
        if (triggerCollider == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Component<SphereCollider> not found");
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Component<Rigidbody> not found");

        triggerCollider.radius = RadiusTrigger.x;
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            target = collider.transform;
            triggerCollider.radius = RadiusTrigger.y;
            switch (action)
            {
                case E_Action.Idle:
                    action = E_Action.WatchOnTarget;
                    StartTimer(TimeBeforAttack);
                    break;

                case E_Action.WatchOnTarget:
                    break;

                case E_Action.Sleep:
                    action = E_Action.WatchOnTarget;
                    StartTimer(TimeBeforAttack);
                    break;

                case E_Action.Chase:
                    break;

                default:
                    break;
            }
        }
    }

    public virtual void OnTriggerStay(Collider collider)
    {
        if(collider.tag == "Player")
        {
            if (!isTimerActive)
            {

                switch (action)
                {
                    case E_Action.Idle:
                        break;

                    case E_Action.WatchOnTarget:
                        action = E_Action.Chase;
                        rigidbody.isKinematic = false;
                        Walke();
                        break;

                    case E_Action.Sleep:
                        break;

                    case E_Action.Chase:
                        break;

                    case E_Action.Attack:
                        rigidbody.velocity = Vector3.zero;
                        rigidbody.isKinematic = true;
                        Attack();
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            switch (action)
            {
                case E_Action.Idle:
                    break;
                case E_Action.WatchOnTarget:
                    target = null;
                    action = E_Action.Sleep;
                    triggerCollider.radius = RadiusTrigger.x;
                    Sleep();
                    rigidbody.isKinematic = true;
                    break;

                case E_Action.Sleep:
                    rigidbody.isKinematic = true;
                    break;

                case E_Action.Chase:
                    action = E_Action.Sleep;
                    triggerCollider.radius = RadiusTrigger.x;
                    Sleep();
                    rigidbody.isKinematic = true;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        CalculationDirectionSprite(target);

        Timer();

        if(target == null)
        {
        }
        else
        {
            if (action == E_Action.Chase)
            {
                targetPos = target.position;
                thisPos = transform.position;

                if ((thisPos - targetPos).sqrMagnitude > DistanceAttack)
                {
                    normalizeMove = (targetPos - thisPos).normalized;
                    directionMove = transform.forward * normalizeMove.z + transform.right * normalizeMove.x;
                    rigidbody.velocity = new Vector3(directionMove.x * SpeedMove, rigidbody.velocity.y, directionMove.z * SpeedMove);
                }
                else
                {
                    action = E_Action.Attack;
                }
            }
        }
    }

    //public virtual void AttackDamage() { }

    public virtual void AttackEnd()
    {
        rigidbody.isKinematic = false;
        if (target.GetComponent<CharacterControl>().Heart > 0)
        {
            action = E_Action.Chase;
            Walke();
        }
        else
        {
            action = E_Action.Sleep;
            Sleep();
        }
    }

    private void StartTimer(float time)
    {
        timer = time;
        isTimerActive = true;
    }

    private void Timer()
    {
        if (isTimerActive)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
            {
                timer = 0;
                isTimerActive = false;
            }
        }
    }

    private void CalculationDirectionSprite(Transform target)
    {
        if (target != null)
        {
            if ((camera.WorldToScreenPoint(target.position) - camera.WorldToScreenPoint(transform.position)).x > 0)
                center.localScale = new Vector3(-1, 1, 1);
            else
                center.localScale = new Vector3(1, 1, 1);
        }
    }
}

public enum E_Action { Idle, Sleep, WatchOnTarget, Chase, Attack}

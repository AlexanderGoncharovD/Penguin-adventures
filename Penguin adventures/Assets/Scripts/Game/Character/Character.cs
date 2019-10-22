using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { NONE, JUMP, FLYING }

public class Character : Animations
{
    public float moveForce = 3f;
    private bool isMove;
    public bool IsMove { get => isMove; set { isMove = value; if (value == false) Idle(); } }



    [Header("Jump settings")]
    public float jumpImpuls = 5.25f; // Сила толчка вверх
    public int countJumps = 2; // Максимальное количество одновременных прыжков (двойной прыжок)
    public float dragForce = 5f; // Сопроивление падения при планировании
    
    private Joystick _joystick;
    private State _state;
    private Rigidbody _rb;
    private Vector3 _position;
    private bool _isGround;
    private int _curCountJumps;
    private CapsuleCollider _collider;
    private float _colliderHeight;
    private ContactPoint _contacPointGround;

    public void Start()
    {
        GameLevel.character = gameObject;
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError("(Character.cs): " + gameObject.name + " Component<Rigidbody> not found");

        _joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        if (_joystick == null)
            Debug.LogError("(Character.cs): " + gameObject.name + " Component<Joystick> not found");

        _collider = GetComponent<CapsuleCollider>();
        if (_collider == null)
            Debug.LogError("(Character.cs): " + gameObject.name + " Component<CapsuleCollider> not found");
        _colliderHeight = _collider.height;
        _position = transform.position;


    }

    public void FixedUpdate()
    {
        if (IsMove == true)
        {
            Vector3 direction = transform.forward * _joystick.vertical + transform.right * _joystick.horizontal;
            _rb.MovePosition(_position + direction * moveForce * Time.fixedDeltaTime);
            flip = _joystick.horizontal;
            if (direction == Vector3.zero)
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            if (_rb.drag > 0f)
                _rb.drag -= dragForce * 2 * Time.fixedDeltaTime;
        }
    }

    public void JumpButton()
    {
        if (IsMove == true)
        {
            if (_curCountJumps < countJumps)
            {
                _curCountJumps++;
                switch (_curCountJumps)
                {
                    case 1:
                        _rb.velocity = Vector3.up * jumpImpuls;
                        break;
                    case 2:
                        _rb.velocity = Vector3.up * jumpImpuls * 0.75f;
                        break;
                    default:
                        break;
                }
                Jump(_curCountJumps);
                _isGround = false;
            }
            else
            {
                if (_state == State.FLYING)
                    _rb.drag = dragForce;
            }
        }
    }

    override public void Update()
    {
        _position = transform.position;

        base.Update();
    }

    void LateUpdate()
    {
        if (_isGround)
        {
            if (_state != State.NONE)
            {
                _state = State.NONE;
                _rb.drag = 0f;
                _curCountJumps = 0;
            }
            if (IsMove == true)
                Walk();
        }
        else
        {
            if (_rb.velocity.y > 0.2f)
            {
                if (_state != State.JUMP)
                    _state = State.JUMP;
            }
            else
            {
                if (_state != State.FLYING)
                    _state = State.FLYING;
            }
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (_isGround == false)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.point.y < _position.y)
                {
                    _contacPointGround = contact;
                    _isGround = true;
                    JumpLanded();
                }
            }
        }
    }

    public void WentBeyondTheLocation()
    {
        IsMove = false;
        //_rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        Died();
    }
}

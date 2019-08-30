using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverArm : MonoBehaviour
{

    public bool State;
    public GameObject Text;

    private Animator animator;
    private SphereCollider sphereCollider;
    private bool isDetectPlayer;
    private float radius;
    private Transform player;
    private int layerMask = 1 << 8;
    private Vector2 startPos;
    private Camera camera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        animator.SetBool("State", State);
        radius = sphereCollider.radius;
    }

    private void Update()
    {
        if (isDetectPlayer)
        {
            if ((transform.position - player.position).sqrMagnitude > radius)
            {
                OnTriggerExit(player.GetComponent<Collider>());
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        // Отслеживание действия касания
                        switch (touch.phase)
                        {
                            // Если только что докаснулся до экрана
                            case TouchPhase.Began:
                                startPos = touch.position;
                                break;

                            // Если палец был убран с экрана
                            case TouchPhase.Ended:
                                // Если отпускание пальца лежит в доступном радиусе (1% от высоты экрана) от прикосновения пальца до экрана
                                if ((touch.position - startPos).magnitude <= Screen.height * 0.01f)
                                {

                                    RaycastHit hit;
                                    if (Physics.Raycast(camera.ScreenPointToRay(startPos), out hit, Mathf.Infinity, layerMask))
                                    {
                                        State = !State;
                                        animator.SetBool("State", State);
                                    }
                                    startPos = Vector2.zero;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.transform;
            isDetectPlayer = true;
            Text.SetActive(isDetectPlayer);
            sphereCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
            isDetectPlayer = false;
            Text.SetActive(isDetectPlayer);
            sphereCollider.enabled = true;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float Height = 3,
        Distance = 10,
        smooth = 2.0f,
        AngleRotation = 45.0f;

    private Transform character;
    private float rotationTarget;
    private float angle;
    private bool isSwipe;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = new Vector3(0, character.position.x + Height, character.position.z - Distance);
    }

    private void Update()
    {
        TouchScreen();


        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotationTarget += AngleRotation;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            rotationTarget -= AngleRotation;
        }
    }

    Vector2 startPos;
    private void TouchScreen()
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

                    // Если было совершено перемещение пальцем
                    case TouchPhase.Moved:
                        // Если это был быстрый свайп
                        if (touch.deltaPosition.magnitude / touch.deltaTime >= 600)
                        {
                            if (!isSwipe)
                            {
                                if (touch.position.y > Screen.height / 2.75f && startPos.y > Screen.height / 2.75f)
                                {
                                    // Если свайп вправо и длина свайпа больше 15% ширины экрана
                                    if (touch.position.x > startPos.x && touch.position.x - startPos.x > Screen.width * 0.15f)
                                    {
                                        rotationTarget += AngleRotation;
                                        isSwipe = true;
                                    }
                                    if (touch.position.x < startPos.x && startPos.x - touch.position.x > Screen.width * 0.15f)
                                    {
                                        rotationTarget -= AngleRotation;
                                        isSwipe = true;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        break;

                    // Если палец был убран с экрана
                    case TouchPhase.Ended:
                        // Если отпускание пальца лежит в доступном радиусе (1% от высоты экрана) от прикосновения пальца до экрана
                        if ((touch.position - startPos).magnitude <= Screen.height * 0.01f)
                        {
                            //OneTouch();
                        }
                        startPos = Vector2.zero;
                        isSwipe = false;
                        break;
                }
            }
        }
        Quaternion rot = Quaternion.Euler(0, rotationTarget, 0);
        character.rotation = Quaternion.Slerp(character.rotation, rot, smooth * Time.deltaTime);
    }
}

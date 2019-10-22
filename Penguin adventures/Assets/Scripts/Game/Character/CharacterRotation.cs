using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public float smooth = 2f;
    public float angleRotation = 90f;

    private Vector2 _startPos;
    private bool _isSwipe;
    private Transform _character;
    private float _rotationTarget;

    public delegate void HintRotationCamera(bool value);
    private HintRotationCamera listOfHintRotationCamera;

    public void RegisterEventCameraRotation(HintRotationCamera listener) => listOfHintRotationCamera += listener;

    private void Start()
    {
        _character = transform;
        AssignParentToCamera(transform);
    }

    public void AssignParentToCamera(Transform parent)
        =>
        GameObject.FindGameObjectWithTag("MainCamera").transform.parent = parent;

    private void Update()
    {
        TouchScreen();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _rotationTarget += angleRotation;
            if (listOfHintRotationCamera != null)
                listOfHintRotationCamera(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
            _rotationTarget -= angleRotation;

    }

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
                        _startPos = touch.position;
                        break;

                    // Если было совершено перемещение пальцем
                    case TouchPhase.Moved:
                        // Если это был быстрый свайп
                        if (touch.deltaPosition.magnitude / touch.deltaTime >= 600)
                        {
                            if (!_isSwipe)
                            {
                                if (touch.position.y > Screen.height / 2.75f && _startPos.y > Screen.height / 2.75f)
                                {
                                    // Если свайп вправо и длина свайпа больше 15% ширины экрана
                                    if (touch.position.x > _startPos.x && touch.position.x - _startPos.x > Screen.width * 0.1f)
                                    {
                                        _rotationTarget += angleRotation;
                                        _isSwipe = true;
                                    }
                                    if (touch.position.x < _startPos.x && _startPos.x - touch.position.x > Screen.width * 0.1f)
                                    {
                                        _rotationTarget -= angleRotation;
                                        _isSwipe = true;
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
                        if ((touch.position - _startPos).magnitude <= Screen.height * 0.01f)
                        {
                            //OneTouch();
                        }
                        _startPos = Vector2.zero;
                        if (_isSwipe)
                        {
                            if (listOfHintRotationCamera != null)
                                listOfHintRotationCamera(true);
                            _isSwipe = false;
                        }
                        break;
                }
            }
        }
        Quaternion rot = Quaternion.Euler(0, _rotationTarget, 0);
        _character.rotation = Quaternion.Slerp(_character.rotation, rot, smooth * Time.deltaTime);
    }


}

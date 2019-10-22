using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool isActive;
    public Teleport targetTeleport;
    public float teleportTime = 1f;
    [HideInInspector]
    public ParticleSystem particle;

    private Transform _character;
    private float _timer;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        if (targetTeleport == null)
            Debug.LogError("(Teleport.cs): " + gameObject.name + " (GO) targetTeleport is Empty");
    }

    private void Update()
    {
        if (isActive)
        {
            if (_character != null)
            {
                if (_timer > 0f)
                {
                    _timer -= Time.deltaTime;
                }
                else
                {
                    _character.position = targetTeleport.transform.position;
                    _character = null;
                    targetTeleport.particle.loop = false;
                    targetTeleport.isActive = false;
                    isActive = false;
                    particle.loop = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _timer = teleportTime;
            _character = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _character = null;
            _timer = 0;
        }
    }

}

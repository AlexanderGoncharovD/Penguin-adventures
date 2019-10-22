using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    [SerializeField] protected int value = 1;
    [SerializeField] private GameObject effect;

    protected virtual void Start() { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }

    protected virtual void CreateEffect()
    {
        if (effect == null)
            Debug.LogWarning("(Name: '" + gameObject.name + "'): No effect");
        else
            Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

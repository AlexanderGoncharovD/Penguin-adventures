using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_TypeItem { None, Star, Food, Heart, Key }

public class Item : MonoBehaviour
{
    [SerializeField] private E_TypeItem type = E_TypeItem.None;
    [SerializeField] private int Value;
    [SerializeField] private GameObject EffectUse;
    private Global global;

    private void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
        if (global == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Scripte 'Global.cs' not found");

        if (type == E_TypeItem.None)
            Debug.LogError("(Name: '" + gameObject.name + "'): Item type not defined");
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var character = other.GetComponent<CharacterControl>();
            switch (type)
            {
                case E_TypeItem.None:
                    Debug.LogError("(Name: '" + gameObject.name + "'): Item type not defined");
                    break;

                case E_TypeItem.Star:
                    global.AddStar(Value);
                    break;

                case E_TypeItem.Food:
                    character.EatFood(Value);
                    break;

                case E_TypeItem.Heart:
                    global.AddHeart(Value);
                    break;

                case E_TypeItem.Key:
                    global.AddKey(Value);
                    break;

                default:
                    break;
            }
            CreateEffect();
        }
    }

    private void CreateEffect()
    {
        if (EffectUse == null)
            Debug.LogWarning("(Name: '" + gameObject.name + "'): No effect");
        else
            Instantiate(EffectUse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

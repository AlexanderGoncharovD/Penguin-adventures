using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBonuse
{
    Star,
    Fish,
    Heart,
}

public class BonusProperties : MonoBehaviour
{
    public TypeBonuse Type;
    public GameObject Effect;

    private Global global;

    private void Awake()
    {
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Effect != null)
            {
                var newEffect = Instantiate(Effect, transform.position, Quaternion.identity);
            }
            if (Type == TypeBonuse.Star)
            {
                global.AddStar();
            }
            else if (Type == TypeBonuse.Fish)
            {
                other.GetComponent<CharacterControl>().EatFish();
            }
            else if (Type == TypeBonuse.Heart)
            {
                global.AddHeart();
            }
            Destroy(gameObject);
        }
    }
}

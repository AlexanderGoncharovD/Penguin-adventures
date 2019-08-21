using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBonuse
{
    Star,
    Fish,
}

public class BonusProperties : MonoBehaviour
{
    public TypeBonuse Type;
    public GameObject Effect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Effect != null)
            {
                var newEffect = Instantiate(Effect, transform.position, Quaternion.identity);
                if (Type == TypeBonuse.Fish)
                {
                    newEffect.transform.parent = other.transform;
                    other.GetComponent<CharacterControl>().EatFish();
                }
            }
            Destroy(gameObject);
        }
    }
}

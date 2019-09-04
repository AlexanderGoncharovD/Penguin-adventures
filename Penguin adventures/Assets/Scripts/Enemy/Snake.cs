using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    public AudioClip Audio_Hiss;


    public void PlaySoundHiss()
    {
        audio.PlayOneShot(Audio_Hiss);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        if (collider.tag == "Player")
        {
            animator.Play("Hiss");
        }
    }

    public void AttackDamage()
    {
        if ((thisPos - target.position).sqrMagnitude <= DistanceDamage)
        {
            target.GetComponent<CharacterControl>().Damage(Damage, "Snake damage");
            audio.PlayOneShot(Audio_Attack);
        }
    }
}

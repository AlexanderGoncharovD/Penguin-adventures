using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    protected override void Start()
    {
        base.Start();
        GameLevel.numberOfStarsAtTheLevel++;
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameLevel.CollectStar(value);
            CreateEffect();
        }
    }
}

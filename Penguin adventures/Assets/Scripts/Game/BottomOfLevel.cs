using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomOfLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameLevel.character.GetComponent<Character>().WentBeyondTheLocation();
        }
    }
}

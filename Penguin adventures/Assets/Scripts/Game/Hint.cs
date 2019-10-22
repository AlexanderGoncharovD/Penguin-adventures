using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public GameObject hint;
    private bool isRegistred;

    private void Start()
    {
        if (hint == null)
            ErrorFindHint();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (hint != null)
            {
                if (isRegistred == false)
                {
                    GameLevel.character.GetComponent<CharacterRotation>().RegisterEventCameraRotation(new CharacterRotation.HintRotationCamera(DoneActionHint));
                    isRegistred = true;
                }
                hint.SetActive(true);
            }
            else
                ErrorFindHint();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (hint != null)
                hint.SetActive(false);
            else
                ErrorFindHint();
        }
    }

    private void DoneActionHint(bool value)
    {
        hint.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    private void ErrorFindHint() => Debug.LogError("(Name: '" + gameObject.name + "'): No hint object selected");
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
    public PortalAssignment assignment;

    private GameObject _character;
    private Animator _anim;

    public delegate void GameCompleted(bool value);
    private GameCompleted listOfListenerGameCompleted;

    public void RegisterListenerGameCompleted(GameCompleted listener)
        => listOfListenerGameCompleted += listener;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("(LevelTeleport.cs): " + gameObject.name + " Component<Animator> not found");
        

        if (assignment == PortalAssignment.NONE)
            Debug.LogError("(LevelTeleport.cs): " + gameObject.name + " Portal purpose not defined");
        else if (assignment == PortalAssignment.STARTING_POINT)
        {
            GameLevel.character.transform.parent = transform.Find("Character").transform;
            _anim.Play("emergence");
            Invoke("ActivateCharacter", 1f);
        }

    }

    private void ActivateCharacter()
    {
        GameLevel.character.transform.parent = null;
        CharacterActivateAndDisactivate(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (assignment == PortalAssignment.END_POINT)
        {
            GameLevel.character.GetComponent<CharacterRotation>().AssignParentToCamera(null);
            CharacterActivateAndDisactivate(false);
            GameLevel.character.transform.parent = transform.Find("Character").transform;
            _anim.Play("emergenceRevers");
            if (listOfListenerGameCompleted != null)
                listOfListenerGameCompleted(true);
        }
    }

    private void CharacterActivateAndDisactivate(bool value)
    {
        GameLevel.character.GetComponent<Character>().IsMove = value;
        GameLevel.character.GetComponent<CharacterRotation>().enabled = value;
        GameLevel.character.GetComponent<Rigidbody>().isKinematic= !value;
    }
}

public enum PortalAssignment { NONE, STARTING_POINT, END_POINT,}

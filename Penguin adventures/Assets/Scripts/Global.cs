using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour
{

    public int Star, MaxHeart, Key;
    public Text T_Star, T_Heart, T_Key;

    private CharacterControl character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControl>();
        if (character == null)
            Debug.LogError("(Name: '" + gameObject.name + "'): Scripte 'CharacterControl.cs' not found");
        else
        {
            T_Heart.text = character.Heart + "";
        }
    }

    public void AddStar(int value)
    {
        Star += value;
        T_Star.text = Star + "";
    }

    public void AddHeart(int value)
    {
        if (character.Heart > 0)
        {
            MaxHeart += value;
            character.Heart += value;
            T_Heart.text = character.Heart + "";
        }
    }

    public void AddKey(int value)
    {
        Key += value;
        T_Key.text = "x" + Key;
    }

    public void UpdateHeart(int heart) => T_Heart.text = heart + "";

    public void IncreaseHeart(int number)
    {
        if (character.Heart < MaxHeart)
        {
            character.Heart += number;
            T_Heart.text = character.Heart + "";
        }
    }
}

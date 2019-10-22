using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDeath : ResultWindowGame
{
    private GameObject windowDeath;

    void Start()
    {
        GameLevel.RegisterForCharacterDied(new GameLevel.CharacterDied(CharacterDiedWindow));
        windowDeath = transform.Find("window").gameObject;
    }

    void Update()
    {
        
    }

    private void CharacterDiedWindow(bool value)
    {
        ShowWindow(windowDeath, 0.25f);
    }
}

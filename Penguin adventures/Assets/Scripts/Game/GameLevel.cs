using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameLevel
{
    public static GameObject character;

    public delegate void CharacterDied(bool value);
    private static CharacterDied listOfListenerCharacterDied;
    public static void RegisterForCharacterDied(CharacterDied listener) 
        => listOfListenerCharacterDied += listener;
   
    private static bool isCharacterDied;
    public static bool IsCharacterDied
    {
        get => isCharacterDied;
        set
        {
            isCharacterDied = value;
            if (isCharacterDied == true)
            {
                if (listOfListenerCharacterDied != null)
                    listOfListenerCharacterDied(true);
            }
        }
    }

    public delegate void CountOfStarsCollected(int value);
    private static CountOfStarsCollected listOfListenerForStars;
    public static int numberOfStarsAtTheLevel;
    private static int countOfStars;
    public static int CountOfStars
    {
        get => countOfStars;
        set
        {
            countOfStars = value;
            if(listOfListenerForStars != null)
                listOfListenerForStars(countOfStars);
        }
    }

    public static void CollectStar(int value) => CountOfStars += value;

    public static void RegisterForCountOfStarsCollected(CountOfStarsCollected listener)
        => listOfListenerForStars += listener;
    
    public static void RestartLevel()
    {
        CountOfStars = 0;
        numberOfStarsAtTheLevel = 0;
        listOfListenerForStars = null;
        listOfListenerCharacterDied = null;
        IsCharacterDied = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

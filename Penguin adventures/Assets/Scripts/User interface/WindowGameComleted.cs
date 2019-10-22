using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WindowGameComleted : ResultWindowGame
{
    public LevelTeleport endTeleport;
    public Text textStars;
    public Text textCompleted;
    private int countStars;
    private int allStars;
    private float completed;
    private GameObject windowGameCompleted;
    private bool isGameComleted;

    private void Start()
    {
        windowGameCompleted = transform.Find("window").gameObject;

        if (endTeleport == null)
            Debug.LogError($"{this}: \"endTeleport\" = null; Impossible to finish level!");
        else
        {
            if (endTeleport.assignment == PortalAssignment.END_POINT)
                endTeleport.RegisterListenerGameCompleted(new LevelTeleport.GameCompleted(GameCompleted));
            else
                Debug.LogError($"{this}: \"endTeleport\" = null; This is not a portal to exit the level!");

        }
    }

    private void GameCompleted(bool value)
    {
        isGameComleted = value;

        countStars = GameLevel.CountOfStars;
        allStars = GameLevel.numberOfStarsAtTheLevel;
        textStars.text = $"{countStars} / {allStars}";

        completed = Mathf.CeilToInt(100f / (float)allStars * (float)countStars);
        textCompleted.text = $"{completed}%";

        ShowWindow(windowGameCompleted, 2.0f);
    }
}

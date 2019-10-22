using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultWindowGame : MonoBehaviour
{
    private GameObject gameWindow;

    public virtual void ShowWindow(GameObject wind, float delay = 0)
    {
        if (delay == 0)
            wind.SetActive(true);
        else
        {
            gameWindow = wind;
            Invoke("ShowWindowDelay", delay);
        }
    }
    private void ShowWindowDelay() => gameWindow.SetActive(true);

    public virtual void RestartLevel()
    {
        GameLevel.RestartLevel();
    }

    public virtual void NextLevel()
    {
        print("Press button 'NextLevel'");
    }

    public virtual void Menu()
    {
        print("Press button 'Menu'");
    }

}

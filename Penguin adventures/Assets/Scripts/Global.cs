using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour
{

    public int Star;
    public Text StarText;

    public int CurHeart,
        MaxHeart;
    public Text CurHeartText;

    private void Start()
    {
        CurHeartText.text = CurHeart + "";
    }

    public void AddStar()
    {
        Star++;
        StarText.text = Star + "";
    }

    public void AddHeart()
    {
        if (CurHeart > 0)
        {
            MaxHeart++;
            CurHeart++;
            CurHeartText.text = CurHeart + "";
        }
    }

    public int ReduceHeart(int damage)
    {
        CurHeart -= damage;
        CurHeartText.text = CurHeart + "";
        return CurHeart;
    }

    public void IncreaseHeart(int number)
    {
        if (CurHeart < MaxHeart)
        {
            CurHeart += number;
            CurHeartText.text = CurHeart + "";
        }
    }
}

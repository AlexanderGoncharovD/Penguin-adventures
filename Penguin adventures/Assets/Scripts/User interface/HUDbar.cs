using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDbar : MonoBehaviour
{
    public Text stars;

    private void Start()
    {
        GameLevel.RegisterForCountOfStarsCollected(new GameLevel.CountOfStarsCollected(UpdateCountOfStars));
    }

    public void UpdateCountOfStars(int countOfStars) => stars.text = countOfStars.ToString();
}


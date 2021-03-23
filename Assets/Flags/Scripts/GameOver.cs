﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    bool gameOver = false;
    Text continued;
    public Color startPanel;
    public Color startText;
    public Color endPanel;
    public Color endText;

    float t = 0;

    public void endGameSuccess()
    {
        gameOver = true;
        continued = GetComponentInChildren<Text>();
        Debug.Log("You won");
        continued.text = "Your accusation was correct! You win :)";

    }

    public void endGameFailure()
    {
        gameOver = true;
        continued = GetComponentInChildren<Text>();
        Debug.Log("You lose");
        continued.text = "Your accusation was not correct. You lose :(";

    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && t < 4.5)
        {
            t = Mathf.PingPong(Time.time, 5);
            GetComponent<Image>().color = Color.Lerp(startPanel, endPanel, t);
            continued.color = Color.Lerp(startText, endText, t);
        }
    }
}

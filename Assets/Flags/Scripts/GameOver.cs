using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public List<GameObject> days;
    bool gameOver = false;
    Text continued;
    public Color startPanel;
    public Color startText;
    public Color endPanel;
    public Color endText;

    float t = 0;

    private int currentDay = 0;
    private bool dayEnded = false;

    public void nextDay()
    {
        if (currentDay < days.Count)
        {
            dayEnded = true;
            currentDay += 1;
            continued = GetComponentInChildren<Text>();
            continued.text = days[currentDay].name;
        }
    }

    public void endGameSuccess()
    {
        gameOver = true;
        continued = GetComponentInChildren<Text>();
        continued.text = "Your accusation was correct! You win :)";

    }

    public void endGameFailure()
    {
        gameOver = true;
        continued = GetComponentInChildren<Text>();
        continued.text = "Your accusation was not correct. You lose :(";

    }

    // Update is called once per frame
    void Update()
    {
        if ((gameOver || dayEnded) && t < 4.5)
        {
            t = Mathf.PingPong(Time.time, 5);
            GetComponent<Image>().color = Color.Lerp(startPanel, endPanel, t);
            continued.color = Color.Lerp(startText, endText, t);
        }
        else if (dayEnded && t > 4.5)
        {
            dayEnded = false;
            t = 0;
            GetComponent<Image>().color = startPanel;
            continued.color = startText;
            days[currentDay - 1].SetActive(false);
            days[currentDay].SetActive(true);
        }
    }
}

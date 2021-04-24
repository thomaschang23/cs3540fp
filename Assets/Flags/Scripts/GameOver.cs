using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static int currentDay = 0;
    public List<GameObject> days;
    public float fadeSpeed = 0.2f;

    private bool gameOver = false;
    private Text continued;
    private bool dayEnded = false;

    public void nextDay()
    {
        if (currentDay < days.Count)
        {
            dayEnded = true;
            currentDay += 1;
            continued = GetComponentInChildren<Text>();
            continued.text = days[currentDay].name;
            FlagManager.flagCount = 0;
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
        if ((gameOver || dayEnded) && GetComponent<Image>().color.a == 0)
        {
            StartCoroutine(FadeUp());
        }
    }

    private IEnumerator FadeUp()
    {
        Image orig;
        Text text;
        while ((orig = GetComponent<Image>()).color.a < 1)
        {
            orig.color = Faded(orig.color, 1);
            text = GetComponentInChildren<Text>(); 
            text.color = Faded(text.color, 1);
            yield return null;
        }
        if (dayEnded)
        {
            dayEnded = false;
            days[currentDay - 1].SetActive(false);
            days[currentDay].SetActive(true);
        }
        StartCoroutine(FadeDown());

    }

    private IEnumerator FadeDown()
    {
        Image orig;
        Text text = GetComponentInChildren<Text>();
        while ((orig = GetComponent<Image>()).color.a > 0)
        {
            orig.color = Faded(orig.color, -1);
            text = GetComponentInChildren<Text>(); 
            text.color = Faded(text.color, -1);
            yield return null;
        }
        orig.color = new Color(orig.color.r, orig.color.g, orig.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    private Color Faded(Color orig, int direction)
    {
        return new Color(orig.r, orig.g, orig.b, (orig.a + (direction * (fadeSpeed * Time.deltaTime))));
    }
}

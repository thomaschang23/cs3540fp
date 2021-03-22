using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public Text pageView;

    private int currentPage;
    private List<List<string>> pages;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            bool areNotesOpen = uiManager.ToggleNotes();
            if (areNotesOpen)
            {
                ShowNotes();
            }
        }
    }

    public void ShowNotes()
    {
        currentPage = 0;
        pages = Paginate(FlagManager.GetFlagTexts());
        DisplayCurrentPage();
    }

    private List<List<string>> Paginate(List<string> texts)
    {
        List<List<string>> ret = new List<List<string>>();
        List<string> curr = new List<string>();
        for (int i = 0; i < texts.Count; i++)
        {
            if (i % 4 == 0)
            {
                curr = new List<string>();
                ret.Add(curr);
            }
            curr.Add(texts[i]);
        }
        return ret;
    }

    private void DisplayCurrentPage()
    {
        if (pages.Count > 0)
        {
            pageView.text = ToPageView(pages[currentPage]);
        }
    }

    private string ToPageView(List<string> texts)
    {
        string ret = "";
        foreach (string text in texts)
        {
            ret += text;
            ret += "\n\n";
        }
        return ret;
    }

    public void NextPage()
    {
        currentPage = Mathf.Min(currentPage + 1, pages.Count - 1);
        DisplayCurrentPage();
    }

    public void PrevPage()
    {
        currentPage = Mathf.Max(0, currentPage - 1);
        DisplayCurrentPage();
    }
}

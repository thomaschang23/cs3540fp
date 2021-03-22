using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public Text pageView;
    public Animator animator;

    private bool areNotesOpen = false;
    private int currentPage;
    private List<List<string>> pages;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleNotes();
        }
    }

    private void ToggleNotes()
    {
        if (areNotesOpen)
        {
            PlayerMovement.mouseChange(1);
            areNotesOpen = false;
            animator.SetBool("IsOpen", false);
        }
        else
        {
            PlayerMovement.mouseChange(-1);
            areNotesOpen = true;
            animator.SetBool("IsOpen", true);
            currentPage = 0;
            pages = Paginate(FlagManager.GetFlagTexts());
            DisplayCurrentPage();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator notesAnimator;
    public Animator dialogueAnimator;
    public Image notesIcon;
    public Image notesIconBkgd;
    public Text notesIconText;
    public Color notesIconInactive;
    public Color notesIconBkgdInactive;
    public Color notesIconTextInactive;
    public Color notesIconActive;
    public Color notesIconBkgdActive;
    public Color notesIconTextActive;

    private bool areNotesOpen = false;
    private bool isDialogueOpen = false;

    public bool ToggleNotes()
    {
        if (areNotesOpen)
        {
            notesIcon.color = notesIconInactive;
            notesIconBkgd.color = notesIconBkgdInactive;
            notesIconText.color = notesIconTextInactive;
            areNotesOpen = false;
            notesAnimator.SetBool("IsOpen", false);
        }
        else
        {
            notesIcon.color = notesIconActive;
            notesIconBkgd.color = notesIconBkgdActive;
            notesIconText.color = notesIconTextActive;
            areNotesOpen = true;
            notesAnimator.SetBool("IsOpen", true);
        }
        ToggleMouseUnlock();
        return areNotesOpen;
    }

    public bool ToggleDialogue()
    {
        if (isDialogueOpen)
        {
            isDialogueOpen = false;
            dialogueAnimator.SetBool("IsOpen", false);
        }
        else
        {
            isDialogueOpen = true;
            dialogueAnimator.SetBool("IsOpen", true);
        }
        ToggleMouseUnlock();
        return isDialogueOpen;
    }

    private void ToggleMouseUnlock()
    {
        if (areNotesOpen || isDialogueOpen)
        {
            PlayerMovement.mouseChange(-1);
        }
        else
        {
            PlayerMovement.mouseChange(1);
        }
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}

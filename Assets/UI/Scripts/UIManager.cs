using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator notesAnimator;
    public Animator dialogueAnimator;

    private bool areNotesOpen = false;
    private bool isDialogueOpen = false;

    public bool ToggleNotes()
    {
        if (areNotesOpen)
        {
            areNotesOpen = false;
            notesAnimator.SetBool("IsOpen", false);
        }
        else
        {
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
}

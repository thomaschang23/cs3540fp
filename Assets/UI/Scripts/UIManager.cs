using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool examining = false;

    public static bool isGamePaused = false;
    public GameObject pauseMenu;

    public GameObject notesCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
                if (areNotesOpen )
                {
                    notesCanvas.SetActive(true);
                    ToggleNotes();
                }
                if (isDialogueOpen)
                {
                    notesCanvas.SetActive(true);
                    ToggleDialogue();
                }
            }
            else
            {
                if (areNotesOpen || isDialogueOpen)
                {
                    notesCanvas.SetActive(false);
                }
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        
        isGamePaused = true;
        ToggleMouseUnlock();
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        
        isGamePaused = false;
        ToggleMouseUnlock();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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
        else if (!examining && !isGamePaused)
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

    public bool ToggleExamine()
    {
        if ((examining || !areNotesOpen) && !isGamePaused) 
        {
            examining = !examining;
        }
        ToggleMouseUnlock();
        return examining;
    }

    public bool ToggleDialogue()
    { 
        if (isDialogueOpen)
        {
            isDialogueOpen = false;
            dialogueAnimator.SetBool("IsOpen", false);
        }
        else if (!isGamePaused)
        {
            isDialogueOpen = true;
            dialogueAnimator.SetBool("IsOpen", true);
        }
        ToggleMouseUnlock();
        return isDialogueOpen;
    }

    private void ToggleMouseUnlock()
    {
        if (areNotesOpen || isDialogueOpen || examining || isGamePaused )
        {
            Debug.Log("OFF");
            PlayerMovement.mouseChange(-1);
        }
        else
        {
            Debug.Log("ON");
            PlayerMovement.mouseChange(1);
        }
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }
}

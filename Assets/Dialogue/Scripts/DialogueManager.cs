using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public float letterWriteSpeed = 0.005f;
    public Text dialogueHint;
    public Text nameView;
    public Text dialogueView;
    public Text promptView1;
    public Text promptView2;
    public Text promptView3;
    public Animator animator;

    public Button button1;
    public Button button2;
    public Button button3;

    private Dialogue tree;
    private int currentNodeIdx;
    private bool isDialogueOpen;
    private string currentDefaultName;

    public void StartDialogue(Dialogue dialogue, string defaultName)
    {
        if (!isDialogueOpen)
        {
            PlayerMovement.mouseChange();

            isDialogueOpen = true;
            animator.SetBool("IsOpen", true);

            currentDefaultName = defaultName;
            tree = dialogue;
            currentNodeIdx = 0;

            DisplayCurrentSentence();
        }
    }

    public void DisplayNextSentence(int promptIdx)
    {
        if (tree.TryGetValue(currentNodeIdx, out DialogueNode node))
        {
            int childrenLength = node.prompts.Length;

            if (childrenLength == 0)
            {
                EndDialogue();
                return;
            }
            else if (childrenLength > promptIdx)
            {
                currentNodeIdx = node.prompts[promptIdx].nextNodeId;
            }
            else
            {
                currentNodeIdx = node.prompts[childrenLength - 1].nextNodeId;
            }
            DisplayCurrentSentence();
        }
        else
        {
            EndDialogue();
        }
    }

    private void DisplayCurrentSentence()
    {
        if (tree.TryGetValue(currentNodeIdx, out DialogueNode node))
        {
            List<string> prompts = new List<string>();
            int childrenLength = node.prompts.Length;
            DialoguePrompt[] availablePrompts = new DialoguePrompt[childrenLength];
            int updatedLength = 0;

            for (int i = 0; i < childrenLength; i++)
            {
                if (node.prompts[i].dependant.Equals("") || FlagManager.CheckFlag(node.prompts[i].dependant))
                {
                    availablePrompts[updatedLength] = node.prompts[i];
                    updatedLength++;
                }
            }

            if (updatedLength > 1)
            {
                for (int i = 0; i < updatedLength; i++)
                {
                    prompts.Add((i + 1).ToString() + ". " + availablePrompts[i].text);
                }
            }
            else if (updatedLength == 1)
            {
                if (availablePrompts[0].text == "")
                {
                    prompts.Add("1. (Continue.)");
                }
                else
                {
                    prompts.Add("1. " + availablePrompts[0].text);
                }
            }
            else
            {
                prompts.Add("1. (End.)");
            }

            if (updatedLength == 3)
            {
                promptView3.text = prompts[2];
                promptView2.text = prompts[1];
                button3.gameObject.SetActive(true);
            }

            if (updatedLength < 3)
            {
                promptView2.text = prompts[1];
                button3.gameObject.SetActive(false);
                button2.gameObject.SetActive(true);
            }

            if (updatedLength < 2)
            {
                button2.gameObject.SetActive(false);
            }

            if (node.name != "")
            {
                nameView.text = node.name;
            }
            else
            {
                nameView.text = currentDefaultName;
            }
            promptView1.text = prompts[0];

            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogueView, node.text));

            FlagManager.SetFlag(node.flagId);
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(Text view, string sentence)
    {
        view.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            view.text += letter;
            yield return new WaitForSeconds(letterWriteSpeed);
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isDialogueOpen = false;
        PlayerMovement.mouseChange();
    }

    public bool IsDialogueOpen()
    {
        return isDialogueOpen;
    }

    public static void TriggerDialogue(GameObject npc)
    {
        npc.BroadcastMessage("TriggerDialogue");
    }

    public void ShowDialogueHint()
    {
        dialogueHint.text = "Click to start dialogue.";
    }
    public void HideDialogueHint()
    {
        dialogueHint.text = "";
    }
}

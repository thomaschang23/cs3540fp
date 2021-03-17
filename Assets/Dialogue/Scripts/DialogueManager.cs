using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
  public float letterWriteSpeed = 0.005f;
  public Text dialogueHint;
  public Text nameView;
  public Text dialogueView;
  public Text promptView;
  public Animator animator;

  public Button button1;
  public Button button2;
  public Button button3;

  private Dialogue tree;
  private int currentNodeIdx;
  private bool isDialogueOpen;

  public void StartDialogue(Dialogue dialogue) {
    if (!isDialogueOpen) {
      PlayerMovement.mouseChange();

      isDialogueOpen = true;
      animator.SetBool("IsOpen", true);

      tree = dialogue;
      currentNodeIdx = 0;

      DisplayCurrentSentence();
    }
  }

  public void DisplayNextSentence(int promptIdx) {
    if (tree.TryGetValue(currentNodeIdx, out DialogueNode node)) {
      int childrenLength = node.prompts.Length;

      if (childrenLength == 0) {
        EndDialogue();
        return;
      } else if (childrenLength > promptIdx) {
        currentNodeIdx = node.prompts[promptIdx].nextNodeId;
      } else {
        currentNodeIdx = node.prompts[childrenLength - 1].nextNodeId;
      }
      DisplayCurrentSentence();
    } else {
      EndDialogue();
    }
  }

  private void DisplayCurrentSentence() {
    if (tree.TryGetValue(currentNodeIdx, out DialogueNode node)) {
      string prompts = "";
      int childrenLength = node.prompts.Length;

      if (childrenLength > 1) {
        for (int i = 0; i < childrenLength; i++) {
          prompts += (i + 1).ToString() + ". " + node.prompts[i].text;
          if (i < childrenLength - 1) {
            prompts += "\n";
          }
        }
      } else if (childrenLength == 1) {
        if (node.prompts[0].text == "") {
          prompts = "1. (Continue.)";
        } else {
          prompts = "1. " + node.prompts[0].text;
        }
      } else {
        prompts = "1. (End.)";
      }

      if (node.prompts.Length < 3) {
        button3.gameObject.SetActive(false);
      }

      if (node.prompts.Length < 2) {
        button2.gameObject.SetActive(false);
      }

      nameView.text = node.name;
      promptView.text = prompts;

      StopAllCoroutines();
      StartCoroutine(TypeSentence(dialogueView, node.text));
    } else {
      EndDialogue();
    }
  }

  IEnumerator TypeSentence(Text view, string sentence) {
    view.text = "";
    foreach (char letter in sentence.ToCharArray()) {
      view.text += letter;
      yield return new WaitForSeconds(letterWriteSpeed);
    }
  }

  private void EndDialogue() {
    animator.SetBool("IsOpen", false);
    isDialogueOpen = false;
    PlayerMovement.mouseChange();
  }

  public bool IsDialogueOpen() {
    return isDialogueOpen;
  }

  public static void TriggerDialogue(GameObject npc) {
    npc.BroadcastMessage("TriggerDialogue");
  }

  public void ShowDialogueHint() {
    dialogueHint.text = "Click to start dialogue.";
  }
  public void HideDialogueHint() {
    dialogueHint.text = "";
  }
}

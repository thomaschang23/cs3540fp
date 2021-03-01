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

  private Tree tree;
  private bool isDialogueOpen;

  public void StartDialogue(Dialogue dialogue) {
    if (!isDialogueOpen) {
      PlayerMovement.mouseChange();

      isDialogueOpen = true;
      animator.SetBool("IsOpen", true);

      nameView.text = dialogue.name;
      tree = dialogue.tree;

      DisplayCurrentSentence();
    }
  }

  public void DisplayNextSentence(int prompt) {
    int childrenLength = tree.children.Length;

    if (childrenLength == 0) {
      EndDialogue();
      return;
    } else if (childrenLength > prompt) {
      tree = tree.children[prompt];
    } else {
      tree = tree.children[childrenLength - 1];
    }

    DisplayCurrentSentence();
  }

  private void DisplayCurrentSentence() {
    string prompts = "";
    int childrenLength = tree.children.Length;

    if (childrenLength > 1) {
      for (int i = 0; i < childrenLength; i++) {
        prompts += (i + 1).ToString() + ". " + tree.children[i].prompt;
        if (i < childrenLength - 1) {
          prompts += "\n";
        }
      }
    } else if (childrenLength == 1) {
      if (tree.children[0].prompt == "") {
        prompts = "1. (Continue.)";
      } else {
        prompts = "1. " + tree.children[0].prompt;
      }
    } else {
      prompts = "1. (End.)";
    }

    StopAllCoroutines();
    StartCoroutine(TypeSentence(dialogueView, tree.sentence));
    StartCoroutine(TypeSentence(promptView, prompts));
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

  public Text nameView;
  public Text dialogueView;

  public Animator animator;

  private Queue<string> sentences;

  // Start is called before the first frame update
  void Start() {
    sentences = new Queue<string>();
  }

  public void StartDialogue(Dialogue dialogue) {
    animator.SetBool("IsOpen", true);

    nameView.text = dialogue.name;

    sentences.Clear();

    foreach (string sentence in dialogue.sentences) {
      sentences.Enqueue(sentence);
    }

    DisplayNextSentence();
  }

  public void DisplayNextSentence() {
    if (sentences.Count == 0) {
      EndDialogue();
      return;
    }

    string sentence = sentences.Dequeue();
    StopAllCoroutines();
    StartCoroutine(TypeSentence(sentence));
  }

  IEnumerator TypeSentence(string sentence) {
    dialogueView.text = "";
    foreach (char letter in sentence.ToCharArray()) {
      dialogueView.text += letter;
      yield return null;
    }
  }

  private void EndDialogue() {
    animator.SetBool("IsOpen", false);
  }
}

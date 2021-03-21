using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string defaultName;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform; // TODO: use global static variable in LevelManager
        Vector3 target = new Vector3(
            player.position.x, 
            transform.position.y, 
            player.position.z
        );
        transform.LookAt(target);

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, defaultName);
    }
}

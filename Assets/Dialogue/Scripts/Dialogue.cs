using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue : SerializableDictionaryBase<int, DialogueNode>
{
}

[System.Serializable]
public class DialogueNode
{
    public string name;
    public string flagId = "";
    public string flagNote = "";
    [TextArea(3, 10)]
    public string text;
    public DialoguePrompt[] prompts;
}

[System.Serializable]
public class DialoguePrompt
{
    public string text;
    public string dependant = "";
    public UnityEvent e;
    public int nextNodeId;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class Dialogue : SerializableDictionaryBase<int, DialogueNode>
{
}

[System.Serializable]
public class DialogueNode
{
    public string name;

    public string flagId;

    [TextArea(3, 10)]
    public string text;

    public DialoguePrompt[] prompts;
}

[System.Serializable]
public class DialoguePrompt
{
    public string text;
    public string dependant = "";
    public int nextNodeId;
}

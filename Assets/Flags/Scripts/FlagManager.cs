using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public static class FlagManager : object
{
    public static Flags flags;
    private static List<string> setFlags = new List<string>();

    public static void SetFlag(string flagId)
    {
        if (!CheckFlag(flagId))
            setFlags.Add(flagId);
    }

    public static bool CheckFlag(string flagID)
    {
        return setFlags.Contains(flagID);
    }

    public static List<string> GetFlagTexts()
    {
        List<string> texts = new List<string>();
        foreach (string flagId in setFlags)
        {
            if (flags.TryGetValue(flagId, out FlagNode node))
            {
                texts.Add(node.text);
            }
        }
        return texts;
    }

}

[System.Serializable]
public class Flags : SerializableDictionaryBase<string, FlagNode>
{
}

[System.Serializable]
public class FlagNode
{
    [TextArea(3, 10)]
    public string text;
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public static class FlagManager : object
{
    private static List<(string, string)> setFlags = new List<(string, string)>();
    public static int flagCount = 0;

    public static bool SetFlag(string flagId, string flagNote)
    {
        if (!CheckFlag(flagId))
        {
            setFlags.Add((flagId, flagNote));
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckFlag(string flagId)
    {
        bool ret = false;
        foreach ((string, string) flag in setFlags)
        {
            ret = ret || (flag.Item1 == flagId);
        }
        return ret;
    }

    public static List<string> GetFlagTexts()
    {
        List<string> texts = new List<string>();
        foreach ((string, string) flag in setFlags)
        {
            texts.Add(flag.Item2);
        }
        return texts;
    }

}

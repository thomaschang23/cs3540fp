using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
  public string name;

  public Tree tree;
}

[System.Serializable]
public class Tree  {
  [TextArea(3, 10)]
  public string prompt;

  [TextArea(3, 10)]
  public string sentence;

  public Tree[] children;
}

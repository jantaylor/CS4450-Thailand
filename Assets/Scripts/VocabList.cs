using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/// <summary>
/// A class to contain an array of words of a specified language
/// </summary>
[System.Serializable]
public class VocabList {
  public string language;
  public string[] words;
  public string[] definitions;

  public override string ToString()
  {
    return words.ToString();
  }
}

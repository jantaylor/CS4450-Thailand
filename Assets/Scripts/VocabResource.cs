using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[System.Serializable]
public class VocabResource {
  public string[] languages;
  public string audioRoot;
  public string wordAudioFile;
  public string definitionAudioFile;
  public string imageFile;
  public VocabList[] vocab;

  private int[] randomOrder;
  public int Length { get { return vocab[0].words.Length; } set {}}

  public string GetWord(int languageIndex, int i)
  {
    return vocab[languageIndex].words[randomOrder[i]];
  }

  public string GetDefinition(int languageIndex, int i)
  {
    return vocab[languageIndex].definitions[randomOrder[i]];
  }


  public string GetWordAudioPath(int languageIndex, int i)
  {
    return wordAudioFile.Replace("{LANG}", vocab[languageIndex].language).Replace("{WORD}", GetWord(0, i));
  }

  public string GetDefinitionAudioPath(int languageIndex, int i)
  {
    return definitionAudioFile.Replace("{LANG}", vocab[languageIndex].language).Replace("{WORD}", GetWord(0, i));
  }

  public string GetImagePath(int i)
  {
    return imageFile.Replace("{WORD}", GetWord(0, i));
  }

  public void GenerateRandomOrder()
  {
		var random = new System.Random();
		randomOrder = new int[Length];

		// Fisher-Yates "inside-out" algorithm
		for (int i = 0; i < Length; i++)
		{
			int j = random.Next(0, i + 1);
			if (i != j)
			{
				randomOrder[i] = randomOrder[j];
			}
			randomOrder[j] = i;
		}
  }

  public override string ToString()
  {
    return "audioRoot: " + audioRoot + "; wordAudioFile: " + wordAudioFile + "; vocab: " + vocab.ToString();
  }

}

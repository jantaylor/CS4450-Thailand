using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour {

	public string word;

	private VocabController vocabController;
	private Text text;

	void Awake() {
		vocabController = UnityEngine.Object.FindObjectOfType<VocabController>();
		text = GetComponent<Text>();
		UpdateWord("?");
	}

	/// <summary>
	/// Updates the word display.
	/// </summary>
	public void UpdateWord(string word)
	{
		this.word = word;
		text.text = word;
	}

	public void OnPointerUp()
	{
		vocabController.ShowLanguageHelp();
	}
}

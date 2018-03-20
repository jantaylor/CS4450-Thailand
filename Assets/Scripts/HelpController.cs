using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour {

	public string word;

	private VocabController vocabController;
	private TextMesh textMesh;
	private BoxCollider2D box;

	void Awake() {
		vocabController = GameObject.Find("Vocab Container").GetComponent<VocabController>();
		textMesh = GetComponent<TextMesh>();
		box = GetComponent<BoxCollider2D>();
		UpdateWord("?");
	}

	/// <summary>
	/// Updates the word display.
	/// </summary>
	public void UpdateWord(string word)
	{
		this.word = word;
		textMesh.text = word;
		UpdateSize();
	}


	/// <summary>
	/// Also resizes the box collider so it is always the same size as the displayed word.
	/// </summary>
	public void UpdateSize()
	{
	// Update the collider size to match the word size.
	var bounds = textMesh.GetComponent<Renderer>().bounds;
	box.size = bounds.size / transform.localScale.x;
	}

	void OnMouseUp()
	{
		vocabController.ShowLanguageHelp();
	}
}

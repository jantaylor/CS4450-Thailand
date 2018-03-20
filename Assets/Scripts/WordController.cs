using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour {

	public AudioClip audioClip;
	public string word;

	private VocabController vocabController;
	private GameObject audioButton;
	private AudioSource audioPlayer;
	private TextMesh textMesh;
	private BoxCollider2D box;

	void Awake() {
		vocabController = GameObject.Find("Vocab Container").GetComponent<VocabController>();
		audioButton = transform.Find("Word Audio Button").gameObject;
		audioPlayer = GameObject.Find("Audio Player").GetComponent<AudioSource>();
		textMesh = GetComponent<TextMesh>();
		box = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Updates the word display and the sound/image resource.
	/// </summary>
	public void UpdateWord(string word, string audioPath)
	{
		this.word = word;
		textMesh.text = word;
		audioClip = Resources.Load<AudioClip>(audioPath);
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

	// move the audio icon to the end of the word
	audioButton.transform.position = new Vector2((float)(box.bounds.size.x / 2 + audioButton.GetComponent<Renderer>().bounds.size.x / 2 * 1.5), (float)audioButton.transform.position.y);
	}

	/// <summary>
	/// Plays the sound of the word clicked/tapped.
	/// </summary>
	public void PlaySound()
	{
			if (audioPlayer.isPlaying)
			{
				audioPlayer.Stop();
			}
			// audioClip.Speed = 0.5;
			audioPlayer.PlayOneShot(audioClip);
			vocabController.ShowLanguageHelp();
	}

	void OnMouseUp()
	{
		PlaySound();
	}
}

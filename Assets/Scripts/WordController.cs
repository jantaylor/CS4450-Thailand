using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour {

	public AudioClip audio;
	public string word;

	private AudioSource audioPlayer;
	private TextMesh textMesh;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		audioPlayer = GameObject.Find("Audio Player").GetComponent<AudioSource>();
		textMesh = GetComponent<TextMesh>();
		box = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Updates the word display and the sound/image resource.  
	/// Also resizes the box collider so it is always the same size as the displayed word.
	/// </summary>
	public void UpdateWord(string word, string audioPath)
	{
		this.word = word;
		textMesh.text = word;

		// Update the hotpoint size to match the word size.
		var bounds = textMesh.GetComponent<Renderer>().bounds;
		box.size = bounds.size / transform.localScale.x;

		audio = Resources.Load<AudioClip>(audioPath);
	}

	/// <summary>
	/// Plays the sound of the word clicked/tapped.
	/// </summary>
	void OnMouseUp()
	{
		if (audioPlayer.isPlaying)
		{
			audioPlayer.Stop();
		}
		audioPlayer.PlayOneShot(audio);
	}
}

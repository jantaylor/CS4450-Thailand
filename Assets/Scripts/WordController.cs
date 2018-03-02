using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour {

	public AudioClip audio;
	public string word;

	private GameObject audioPlayer;
	private TextMesh textMesh;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		audioPlayer = GameObject.Find("Audio Player");
		textMesh = GetComponent<TextMesh>();
		box = GetComponent<BoxCollider2D>();
		UpdateWord(word);
	}

	// Update is called once per frame
	void Update () {

	}

	void UpdateWord(string word)
	{
		this.word = word;
		textMesh.text = word;

		// Update the hotpoint size to match the word size.
		var bounds = textMesh.GetComponent<Renderer>().bounds;
		box.size = bounds.size / transform.localScale.x;
	}

	void OnMouseUp()
	{
		Debug.Log("Audio");
		audioPlayer.GetComponent<AudioSource>().PlayOneShot(audio);
	}
}

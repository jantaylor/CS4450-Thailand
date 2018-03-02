using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour {

	public AudioClip audio;
	public string word;

	private SpriteRenderer image;
	private GameObject audioPlayer;
	private TextMesh textMesh;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		image = GameObject.Find("Vocab Image").GetComponent<SpriteRenderer>();
		audioPlayer = GameObject.Find("Audio Player");
		textMesh = GetComponent<TextMesh>();
		box = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void UpdateWord(string word, string audioPath, string imagePath)
	{
		this.word = word;
		textMesh.text = word;

		// Update the hotpoint size to match the word size.
		var bounds = textMesh.GetComponent<Renderer>().bounds;
		box.size = bounds.size / transform.localScale.x;

		audio = Resources.Load<AudioClip>(audioPath);
		image.sprite = Resources.Load<Sprite>(imagePath);
	}

	void OnMouseUp()
	{
		audioPlayer.GetComponent<AudioSource>().PlayOneShot(audio);
	}
}

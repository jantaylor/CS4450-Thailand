﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour {

	public AudioClip audioClip;
	public string word;

	private VocabController vocabController;
	private GameObject audioButton;
	private Text uiText;
	private BoxCollider2D box;
	private string audioPath;

	void Awake() {
		vocabController = GameObject.FindObjectOfType<VocabController>();
		audioButton = transform.Find("Word Audio Button").gameObject;
		uiText = GetComponent<Text>();
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
		uiText.text = word;
		audioClip = Resources.Load<AudioClip>(audioPath);
		this.audioPath = audioPath;
	}

	/// <summary>
	/// Plays the sound of the word clicked/tapped.
	/// </summary>
	public void PlaySound()
	{
		// vocabController will play the sound and unlock the forward progress
		if (vocabController != null)
		{
			vocabController.PlaySound(audioPath);
		}
		else
		{
			AudioPlaybackManager.PlaySound(audioPath);
		}
	}
}

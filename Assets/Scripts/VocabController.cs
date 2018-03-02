using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabController : MonoBehaviour {

	public string[] englishWords;
	public string[] thaiWords;
	public AudioClip[] englishAudioClips;
	public AudioClip[] thaiAudioClips;

	private int currentIndex;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void proceed (int i)
	{
		currentIndex += i;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlaybackManager : MonoBehaviour {

	private static AudioSource audioPlayer;
	private GameObject primaryAudioObject;

	void Awake()
	{
		audioPlayer = GameObject.Find("Audio Playback UI").GetComponent<AudioSource>();
	}

	public void PlaySound()
	{
		if (primaryAudioObject != null)
		{
			// primaryAudioObject.GetComponent<SoundPlayer>.PlaySound();
		}
	}

	public static void PlaySound(string clipName)
	{
		AudioClip clip = Resources.Load<AudioClip>(clipName + "__speed_" + GameState.Instance.ActiveAudioSpeed);
		clip = (clip != null)? clip : Resources.Load<AudioClip>(clipName);

		if (audioPlayer.isPlaying)
		{
			audioPlayer.Stop();
		}
		audioPlayer.PlayOneShot(clip);
	}


}

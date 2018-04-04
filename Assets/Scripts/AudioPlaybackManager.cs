using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlaybackManager : MonoBehaviour {

	private static AudioSource audioPlayer;
	private static string defaultClip;

	void Awake()
	{
		audioPlayer = GameObject.Find("Audio Playback UI").GetComponent<AudioSource>();
	}

	public static void SetDefaultClip(string clip)
	{
		defaultClip = clip;
	}

	public void PlaySound()
	{
		if (defaultClip != null)
		{
			PlaySound(defaultClip);
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

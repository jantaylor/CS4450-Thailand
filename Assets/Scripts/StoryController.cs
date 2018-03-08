﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : MonoBehaviour {

    /// <summary>
    /// The audiosource for playing sound
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The first word that is read separately
    /// </summary>
    public AudioClip word;

    /// <summary>
    /// The second word that is read separately
    /// </summary>
    public AudioClip word2;

    /// <summary>
    /// The sentence that is read
    /// </summary>
    public AudioClip sentence;

    /// <summary>
    /// Play Word
    /// </summary>
    public void PlayWordOne() {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.PlayOneShot(word);
    }

    /// <summary>
    /// Play Word 2
    /// </summary>
    public void PlayWordTwo() {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.PlayOneShot(word2);
    }

    /// <summary>
    /// Play sentence
    /// </summary>
    public void PlaySentence() {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.PlayOneShot(sentence);
    }
}

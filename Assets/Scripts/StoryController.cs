using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// The second word that is read separately
    /// </summary>
    public AudioClip word3;

    /// <summary>
    /// The sentence that is read
    /// </summary>
    public AudioClip sentence;

    /// <summary>
    /// Boolean for if the menu is hidden
    /// </summary>
    public bool menuHidden;

    /// <summary>
    /// Game object for the back button
    /// </summary>
    public Image backButton;

    /// <summary>
    /// Game Object for the forward button
    /// </summary>
    public Image forwardButton;

    /// <summary>
    /// Game Object for the menu button
    /// </summary>
    public Image menuButton;

    /// <summary>
    /// Game object for the normal sound sentance clip
    /// </summary>
    public Image normalSound;

    /// <summary>
    /// Game object for the slowed down sentance clip
    /// </summary>
    public Image slowSound;

    /// <summary>
    /// Play sentence when loading the scene
    /// </summary>
    public void Start() {
        HideMenu();
        //PlaySentence(); // Removed on request on sprint 1
    }

    /// <summary>
    /// Show the menu but the back button
    /// </summary>
    public void ShowMenu() {
        int activeScene = GameState.Instance.ActiveScene;
        // don't show the back button on first scenes of story
        if (activeScene != 7 && activeScene != 15 && activeScene != 23 && activeScene != 31) {
            backButton.enabled = true;
        }
        forwardButton.enabled = true;
        //normalSound.enabled = true;
        //slowSound.enabled = true;
    }

    /// <summary>
    /// Hide the menu but the back button
    /// </summary>
    public void HideMenu() {
        backButton.enabled = true;
        forwardButton.enabled = true;
        //normalSound.enabled = true;
        //slowSound.enabled = true;
    }

    /// <summary>
    /// Play Word
    /// </summary>
    public void PlayWordOne() {
        if (audioSource.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        audioSource.PlayOneShot(word);
    }

    /// <summary>
    /// Play Word 2
    /// </summary>
    public void PlayWordTwo() {
        if (audioSource.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        audioSource.PlayOneShot(word2);
    }

    /// <summary>
    /// Play Word 3
    /// </summary>
    public void PlayWordThree() {
        if (audioSource.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        audioSource.PlayOneShot(word3);
    }

    /// <summary>
    /// Play sentence
    /// </summary>
    public void PlaySentence() {
        if (audioSource.isPlaying)
            //audioSource.Stop(); - Sprint 1 fix
            return;
        audioSource.PlayOneShot(sentence);

        // Once the sentance has been played, show the menu to control scene
        ShowMenu();
    }
}

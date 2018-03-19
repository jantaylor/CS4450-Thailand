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
    /// The words that are read separately
    /// </summary>
    public AudioClip word, word2, word3;

    /// <summary>
    /// The sentence that is read
    /// </summary>
    public AudioClip sentence;

    /// <summary>
    /// Boolean for if the menu is hidden
    /// </summary>
    public bool menuHidden;

    /// <summary>
    /// Game objects for UI Menu
    /// </summary>
    public GameObject backButton, forwardButton, menuButton, normalSound, slowSound;

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
        if (!menuHidden)
            return;

        int activeScene = GameState.Instance.ActiveScene;
        // don't show the back button on first scenes of story
        if (activeScene != 0 && activeScene != 8 && activeScene != 16 && activeScene != 24) {
            backButton.gameObject.SetActive(true);
        }
        forwardButton.gameObject.SetActive(true);
        //normalSound.gameObject.SetActive(true);
        //slowSound.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide the menu but the back button
    /// </summary>
    public void HideMenu() {
        if (menuHidden)
            return;

        menuHidden = true;
        backButton.gameObject.SetActive(false);
        forwardButton.gameObject.SetActive(false);
        //normalSound.gameObject.SetActive(false);
        //slowSound.gameObject.SetActive(false);
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
        Invoke("ShowMenu", 3f);
    }
}

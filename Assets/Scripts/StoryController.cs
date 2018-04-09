using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {

    /// <summary>
    /// The words that are read separately
    /// </summary>
    public string word, word2, word3;

    /// <summary>
    /// The sentence that is read
    /// </summary>
    public string sentence;

    /// <summary>
    /// Boolean for if the menu is hidden
    /// </summary>
    public bool menuHidden;

    /// <summary>
    /// Boolean for a check on if the player has completed the scene
    /// </summary>
    public bool sceneCompleted = false;

    /// <summary>
    /// Game objects for UI Menu
    /// </summary>
    public GameObject backButton, forwardButton, menuButton, normalSound;

    /// <summary>
    /// Checkmark button for last scene
    /// </summary>
    public Sprite checkMark;

    /// <summary>
    /// Play sentence when loading the scene
    /// </summary>
    public void Awake() {
        HideMenu();
        //PlaySentence(); // Removed on request on sprint 1
    }

    public void Start() {
        sceneCompleted = false;
    }

    public void Update() {
        if (sceneCompleted && !AudioPlaybackManager.audioPlayer.isPlaying)
            ShowMenu();
    }

    /// <summary>
    /// Show the menu but the back button
    /// </summary>
    public void ShowMenu() {
        if (!menuHidden)
            return;

        menuHidden = false;
        int activeScene = GameState.Instance.ActiveScene;
        // don't show the back button on first scenes of story
        if (activeScene != 0 && activeScene != 8 && activeScene != 16 && activeScene != 24)
            backButton.gameObject.SetActive(true);

        // Change last scene to display checkmark instead
        if (activeScene == 7 || activeScene == 15 || activeScene == 23 || activeScene == 31)
            forwardButton.GetComponent<Image>().sprite = checkMark;

        forwardButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
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
        menuButton.gameObject.SetActive(false);
        //normalSound.gameObject.SetActive(false);
        //slowSound.gameObject.SetActive(false);
    }

    /// <summary>
    /// Play Word
    /// </summary>
    public void PlayWordOne() {
        if (AudioPlaybackManager.audioPlayer.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        AudioPlaybackManager.PlaySound(word);
    }

    /// <summary>
    /// Play Word 2
    /// </summary>
    public void PlayWordTwo() {
        if (AudioPlaybackManager.audioPlayer.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        AudioPlaybackManager.PlaySound(word2);
    }

    /// <summary>
    /// Play Word 3
    /// </summary>
    public void PlayWordThree() {
        if (AudioPlaybackManager.audioPlayer.isPlaying)
            // audioSource.Stop(); - Sprint 1 fix
            return;
        AudioPlaybackManager.PlaySound(word3);
    }

    /// <summary>
    /// Play sentence
    /// </summary>
    public void PlaySentence() {
        if (AudioPlaybackManager.audioPlayer.isPlaying)
            //audioSource.Stop(); - Sprint 1 fix
            return;
        AudioPlaybackManager.PlaySound(sentence);

        // Once the sentance has been played, show the menu to control scene
        sceneCompleted = true;
    }
}

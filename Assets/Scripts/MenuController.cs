using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public StoryController storyController;

    public void Start() {
        storyController = FindObjectOfType<StoryController>();
    }

    /// <summary>
    /// Load next scene
    /// </summary>
    public void LoadPreviousScene() {
        GameState.Instance.LoadPreviousScene();
        storyController.sceneCompleted = false;
        storyController.HideMenu();
    }

    /// <summary>
    /// Load next scene
    /// </summary>
    public void LoadNextScene() {
        GameState.Instance.LoadNextScene();
        storyController.sceneCompleted = false;
        storyController.HideMenu();
	}

    /// <summary>
    /// Load scene based on the activity and round
    /// </summary>
    /// <param name="activity">int 0-3</param>
    /// <param name="round">int 1-4</param>
    public void LoadScene(int activity, int round) {
        GameState.Instance.LoadScene(activity, round);
    }

    /// <summary>
    /// Load the Menu
    /// </summary>
    public void LoadMenu() {
        GameState.Instance.LoadMenu();
    }
}

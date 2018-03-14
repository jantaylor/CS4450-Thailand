using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    /// <summary>
    /// Load next scene
    /// </summary>
    public void LoadPreviousScene() {
        GameState.Instance.LoadPreviousScene();
    }

    /// <summary>
    /// Load next scene
    /// </summary>
    public void LoadNextScene() {
        GameState.Instance.LoadNextScene();		
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

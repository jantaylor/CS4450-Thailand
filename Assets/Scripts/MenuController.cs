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
	

}

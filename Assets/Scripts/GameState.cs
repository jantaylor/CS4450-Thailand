using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    /// <summary>
    /// Create an instance of the game state object in memory
    /// </summary>
    public static GameState Instance;

    /// <summary>
    /// Scene name
    /// </summary>
    [SerializeField]
    private int activeScene;

    private string[] scenes = new string[] {
        "Round 1 Story 1",
        "Round 1 Story 2",
        "Round 1 Story 3",
        "Round 1 Story 4",
        "Round 1 Story 5",
        "Round 1 Story 6",
        "Round 1 Story 7",
        "Round 1 Story 8"
    };

    /// <summary>
    /// Set and get active scene
    /// </summary>
    public int ActiveScene {
        get { return activeScene; }
        set { activeScene = value; }
    }

    /// <summary>
    /// On starting the game
    /// </summary>
    void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        activeScene = 0;
	}

    /// <summary>
    /// As it says, load previous Scene
    /// </summary>
    public void LoadPreviousScene() {
        // Load the Scene
        if (ActiveScene != 0)
            SceneManager.LoadScene(scenes[ActiveScene -= 1]);
    }

    /// <summary>
    /// As it says, load next Scene
    /// </summary>
    public void LoadNextScene() {
        // Load the Scene
        if (ActiveScene != scenes.Length - 1)
            SceneManager.LoadScene(scenes[ActiveScene += 1]);
    }
}

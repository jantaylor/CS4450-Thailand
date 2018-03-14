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

    /// <summary>
    /// Round difficulty 1 - easy, 4 - hard
    /// </summary>
    [SerializeField]
    private int activeRound;

    /// <summary>
    /// List of Round Names hard coded
    /// </summary>
    [SerializeField]
    private string[] scenes = new string[] {
        "Round 1 Story 1",
        "Round 1 Story 2",
        "Round 1 Story 3",
        "Round 1 Story 4",
        "Round 1 Story 5",
        "Round 1 Story 6",
        "Round 1 Story 7",
        "Round 1 Story 8",
        "Round 2 Story 1",
        "Round 2 Story 2",
        "Round 2 Story 3",
        "Round 2 Story 4",
        "Round 2 Story 5",
        "Round 2 Story 6",
        "Round 2 Story 7",
        "Round 2 Story 8"
    };

    /// <summary>
    /// Set and get active scene
    /// </summary>
    public int ActiveScene {
        get { return activeScene; }
        set { activeScene = value; }
    }

    /// <summary>
    /// Get round and return it
    /// </summary>
    /// <returns>int 1-4</returns>
    public int ActiveRound {
        get { return activeRound; }
        set { activeRound = value; }
    }

    /// <summary>
    /// On starting the game, singleton state created
    /// </summary>
    void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set the active state to have an activeScene of 0
    /// Set the active round to be 0 meaning menu, not started
    /// </summary>
    void Start () {
        activeScene = 0; // -1 is menu, 0 is round 1 story 1
        activeRound = 1; // -1 is menu, 1 is round 1
	}

    /// <summary>
    /// Load previous Scene
    /// </summary>
    public void LoadPreviousScene() {
        // Load the Scene
        if (ActiveScene != 0 && ActiveScene != 8 && ActiveScene != 16 && ActiveScene != 24) {
            SceneManager.LoadScene(scenes[ActiveScene -= 1]);
        } else {
            LoadMenu();
        }
    }

    /// <summary>
    /// Load next Scene
    /// </summary>
    public void LoadNextScene() {
        // Load the Scene based on last scene
        if (ActiveScene != 7 && ActiveScene != 15 && ActiveScene != 23 && ActiveScene != 31) {
            SceneManager.LoadScene(scenes[ActiveScene += 1]);
        } else {
            LoadMenu();
        }
    }

    /// <summary>
    /// Load the menu
    /// </summary>
    public void LoadMenu() {
        // Load the menu
        ActiveScene = -1; // menu scene
        ActiveRound = -1; // menu round
        SceneManager.LoadScene("Menu");
    }
}

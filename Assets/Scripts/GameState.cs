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
    private int round;

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
    /// Get round and return it
    /// </summary>
    /// <returns>int 1-4</returns>
    public int GetRound() {
        return round;
    }

    /// <summary>
    /// Set the round value
    /// </summary>
    /// <param name="value">int 1-4</param>
    public void SetRound(int value) {
        round = value;
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
    /// Load previous Scene
    /// </summary>
    public void LoadPreviousScene() {
        // Load the Scene
        if (ActiveScene != 0)
            SceneManager.LoadScene(scenes[ActiveScene -= 1]);
    }

    /// <summary>
    /// Load next Scene
    /// </summary>
    public void LoadNextScene() {
        // Load the Scene
        if (ActiveScene != scenes.Length - 1)
            SceneManager.LoadScene(scenes[ActiveScene += 1]);
    }

    /// <summary>
    /// Load the menu
    /// </summary>
    public void LoadMenu() {
        // Load the menu
        SceneManager.LoadScene("Menu");
    }
}

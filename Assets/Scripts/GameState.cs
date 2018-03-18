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
    /// Language name
    /// </summary>
    [SerializeField]
    private string activeLanguage;

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
    /// Activity currently active, 1 - vocab, 2 - story, 3 - game
    /// </summary>
    [SerializeField]
    private int activeActivity;

    /// <summary>
    /// Story currently active
    /// </summary>
    [SerializeField]
    private string activeStory;

    /// <summary>
    /// Difficulty currently active, 1 - beginner, 2 - intermediate, 3 - advanced
    /// </summary>
    [SerializeField]
    private int activeDifficulty;

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
    /// Get Language and return it
    /// </summary>
    /// <returns>Name of the Language (Scalable to include other languages)</returns>
    public string ActiveLanguage {
        get { return activeLanguage; }
        set { activeLanguage = value; }
    }

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
    /// Get Activity and return it
    /// </summary>
    /// <returns>int 1-3</returns>
    public int ActiveActivity {
        get { return activeActivity; }
        set { activeActivity = value; }
    }

    /// <summary>
    /// Get Story and return it
    /// </summary>
    /// <returns>Name of the Story (Scalable to include other stories)</returns>
    public string ActiveStory {
        get { return activeStory; }
        set { activeStory = value; }
    }

    /// <summary>
    /// Get Difficulty and return it
    /// </summary>
    /// <returns>int 1-3</returns>
    public int ActiveDifficulty {
        get { return activeDifficulty; }
        set { activeDifficulty = value; }
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
        activeLanguage = "thai"; // TODO: Make this not hard-coded
        activeActivity = -1; // -1 is menu, 1 - vocab
        activeRound = -1; // -1 is menu, 1 is round 1
        activeScene = -1; // -1 is menu, 0 is round 1 story 1
        activeStory = "Waterfight for Songkran"; // TODO: Make this not hard-coded
        activeDifficulty = 1; // 1 is Beginner, 2 is Intermediate, 3 is Advanced
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

    /// <summary>
    /// Load scene based on the activity and round
    /// </summary>
    /// <param name="activity">int 1-3</param>
    /// <param name="round">int 1-4</param>
    public void LoadScene(int activity, int round) {
        // Activity: -1 - menu, 1 - vocab, 2 - story, 3 - game
        // Round: -1 - menu, 1 - easy, 2 - secondary
        if (activity == 1) {
            ActiveActivity = activity;
            ActiveRound = round;
            SceneManager.LoadScene("Vocab");
        } else if (activity == 2) {
            ActiveActivity = activity;
            ActiveRound = round;
            switch (round) {
                case 1:
                    ActiveScene = 0;
                    break;
                case 2:
                    ActiveScene = 8;
                    break;
                case 3:
                    ActiveScene = 16;
                    break;
                case 4:
                    ActiveScene = 24;
                    break;
                default:
                    ActiveScene = -1;
                    Debug.Log("Something went wrong. Not passing correct Active Scene/round.");
                    break;
            }
            SceneManager.LoadScene("Round " + round + " Story 1");
        } else if (activity == 3) {
            // TODO
        } else {
            Debug.Log("Something went wrong. Not passing proper activity.");
            LoadMenu();
        }
    }
}

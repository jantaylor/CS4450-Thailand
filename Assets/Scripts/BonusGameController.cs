using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameController : MonoBehaviour {

    /// <summary>
    /// Water particle system
    /// </summary>
    public ParticleSystem water;

    /// <summary>
    /// Sfx array
    /// </summary>
    public AudioClip[] sfx;

    /// <summary>
    /// Sfx to play when gameOver
    /// </summary>
    public AudioClip finishGameSfx;

    /// <summary>
    /// Score text displayed in Thai
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// Current time in level
    /// </summary>
    public float timeLeft = 90;

    /// <summary>
    /// Game Timer object
    /// </summary>
    public Text gameTimer;

    /// <summary>
    /// Final score text object
    /// </summary>
    public Text FinalScoreText;

    /// <summary>
    /// Options Button
    /// </summary>
    public Image options;

    /// <summary>
    /// Signify game is over
    /// </summary>
    public bool gameOver = false;

    /// <summary>
    /// Second stop
    /// </summary>
    private bool stopGame = false;

    /// <summary>
    /// Player's score
    /// </summary>
    private int score = 0;

    private float currentTime = 60;

    private AudioSource audioSource;

    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = "คะแนน:" + score.ToString();
            }
    }

	// Use this for initialization
	void Start () {
        audioSource = FindObjectOfType<AudioSource>();
        score = 0;
        currentTime = 60;
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver) {
            if (Input.GetButtonDown("Fire1"))
                Fire();

            UpdateTime();
        }

        if (gameOver && !stopGame)
            GameOver();
    }

    private void UpdateTime() {
        timeLeft -= Time.deltaTime;
        int time = (int)timeLeft;
        gameTimer.text = time.ToString();
        if (timeLeft <= 0) {
            gameOver = true;
        }
    }

    private void GameOver() {
        audioSource.Stop();
        audioSource.PlayOneShot(finishGameSfx);
        FinalScoreText.text = "ผลคะแนนสุดท้าย: " + score.ToString();
        FinalScoreText.gameObject.SetActive(true);
        options.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameTimer.gameObject.SetActive(false);
        stopGame = true;
    }

    /// <summary>
    /// Restarts/starts particle system and plays sfx
    /// </summary>
    public void Fire() {
        water.Stop();
        water.Play();
        int rand = Random.Range(0, sfx.Length);
        audioSource.PlayOneShot(sfx[rand]);
    }
}

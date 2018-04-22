using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour {

    public float CurrentScene { get; set; }

    public float MaxScenes { get; set; }

    public Slider progressBar;

	// Use this for initialization
	void Start () {
        // Not all scenes have progress bars
        CurrentScene = GameState.Instance.ActiveScene;
        MaxScenes = 8f;

        // Stories are 8 scenes long
        int round = GameState.Instance.ActiveRound;

        // Adjust currentScene based on round for proper progress
        if (round == 1)
            CurrentScene += 1;
        else if (round == 2)
            CurrentScene -= 7;
        else if (round == 3)
            CurrentScene -= 15;
        else if (round == 4)
            CurrentScene -= 23;
        else
            Debug.Log("Invalid Round, MaxScenes not set.");

        progressBar.value = CalculateProgress();
	}

    public float CalculateProgress() {
        return CurrentScene / MaxScenes;
    }
}

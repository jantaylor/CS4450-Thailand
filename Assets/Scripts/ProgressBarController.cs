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
        CurrentScene = GameState.Instance.ActiveScene + 1;

        // Stories are 8 scenes long
        int round = GameState.Instance.ActiveRound;
        if (round == 1)
            MaxScenes = 8f;
        else if (round == 2)
            MaxScenes = 16f;
        else if (round == 3)
            MaxScenes = 24f;
        else if (round == 4)
            MaxScenes = 32f;
        else
            Debug.Log("Invalid Round, MaxScenes not set.");

        progressBar.value = CalculateProgress();
	}

    public float CalculateProgress() {
        return CurrentScene / MaxScenes;
    }
}

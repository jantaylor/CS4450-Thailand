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

        // Stories are 8 scenes long
        MaxScenes = 8f;

        progressBar.value = CalculateProgress();
	}

    public float CalculateProgress() {
        return CurrentScene / MaxScenes;
    }
}

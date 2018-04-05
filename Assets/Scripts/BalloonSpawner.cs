using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour {

    public GameObject[] balloons;

    public float startTime = 2f;

    public float spawnTime = 1.1f;

    public float minX = -100f;

    public float maxX = 100f;

    public float minZ = 90f;

    public float maxZ = 110f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnBalloon", startTime, spawnTime);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void SpawnBalloon() {
        // Choose Random balloon with different weights
        int rand = Random.Range(0, balloons.Length);

        // Choose random X/Z coordinates
        float randx = Random.Range(minX, maxX);
        float randz = Random.Range(minZ, maxZ);

        // Spawn and move the balloon
        GameObject newBalloon = Instantiate(balloons[rand]);
        newBalloon.transform.position = new Vector3(randx, newBalloon.transform.position.y, randz);
    }
}

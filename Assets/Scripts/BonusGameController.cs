using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGameController : MonoBehaviour {

    /// <summary>
    /// Water particle system
    /// </summary>
    public ParticleSystem water;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            Fire();
    }

    /// <summary>
    /// Restarts/starts particle system
    /// </summary>
    public void Fire() {
        water.Stop();
        water.Play();
    }
}

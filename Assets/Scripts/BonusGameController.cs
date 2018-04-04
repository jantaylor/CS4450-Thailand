using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGameController : MonoBehaviour {

    /// <summary>
    /// Water particle system
    /// </summary>
    public ParticleSystem water;

    /// <summary>
    /// Sfx array
    /// </summary>
    public AudioClip[] sfx;

    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = FindObjectOfType<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            Fire();
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

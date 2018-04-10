using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    public int balloonValue;

    public ParticleSystem water;

    private bool isHit = false;

    public AudioSource audioSource;

    public AudioClip waterSFX;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isHit && !water.isPlaying)
            DestroyBalloon();
	}

    public void Explode() {
        if (!isHit) {
            transform.GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            audioSource.PlayOneShot(waterSFX);
            water.Play();
            isHit = true;
        }
    }

    private void DestroyBalloon() {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Floor"))
            Destroy(this.gameObject);
    }
}

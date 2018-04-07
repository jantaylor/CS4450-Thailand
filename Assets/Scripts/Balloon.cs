using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    public int balloonValue;

    public ParticleSystem water;

    private bool isHit = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isHit && !water.isPlaying)
            DestroyBalloon();
	}

    public void Explode() {
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        water.Play();
        isHit = true;
    }

    private void DestroyBalloon() {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Floor"))
            Destroy(this.gameObject);
    }
}

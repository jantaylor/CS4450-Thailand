using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    public int balloonValue;

    public BonusGameController gameController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Floor"))
            Destroy(this.gameObject);
    }

    void OnCollissionEnter(Collision other) {
        Debug.Log("Collider hit");
        if (other.gameObject.CompareTag("Water")) {
            Debug.Log("Balloon Hit");
            gameController.Score += balloonValue;
            Destroy(this.gameObject);
        }
    }
}

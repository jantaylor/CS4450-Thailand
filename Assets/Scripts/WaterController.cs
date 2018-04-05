using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    public BonusGameController gameController;

    void OnParticleCollision(GameObject other) {
        if (other.CompareTag("Balloon")) {
            gameController.Score += other.GetComponent<Balloon>().balloonValue;
            Destroy(other.gameObject);
        }
    }
}

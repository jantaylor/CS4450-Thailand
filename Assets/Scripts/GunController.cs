
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public BonusGameController bonusGameController;

    void Start() {
        bonusGameController = FindObjectOfType<BonusGameController>();
    }

    void Update() {
        if (bonusGameController.gameOver)
            return;

        // Get mouse position and set Z to far back
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 20.0f;

        // Set look position to camera's point and calculate the transform difference
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;

        // Set limits for boundary
        lookPos.x = Mathf.Clamp(lookPos.x, -10f, 8f);
        lookPos.y = Mathf.Clamp(lookPos.y, 0f, 5f);

        // Rotate the axis
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
}

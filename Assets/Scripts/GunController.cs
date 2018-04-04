
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField]
    private float turnRateRadians = 2 * Mathf.PI;

    void Update() {
        // Get mouse position and set Z to far back
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 20.0f;

        // Set look position to camera's point and calculate the transform difference
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;

        // Rotate the axis
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
}

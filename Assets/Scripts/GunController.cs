
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    [SerializeField]
    private float turnRateRadians = 2 * Mathf.PI;

    void Update() {
        // Get mouse click - Input.mousePosition

        //Vector3 targetDirection = Input.mousePosition - transform.position;
        //targetDirection.z = 0f;
        //targetDirection = targetDirection.normalized;

        //Vector3 currentDirection = transform.forward;
        //currentDirection = Vector3.RotateTowards(currentDirection, targetDirection, turnRateRadians * Time.deltaTime, .5f);

        //Quaternion qDirection = new Quaternion();
        //qDirection.SetLookRotation(currentDirection, transform.forward);
        ////qDirection = new Quaternion(qDirection.x, qDirection.y, 0f, 0f);
        //transform.rotation = qDirection;

        var mousePos = Input.mousePosition;
        mousePos.z = 10.0f; //The distance from the camera to the player object
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

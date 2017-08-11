using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurret : MonoBehaviour {

    /*
     * CONTROLS TURRET AND MONITORS FOR PLAYER.
     * 
     * TURRET ROTATES BETWEEN -45 AND 45 DEGREES, Z AXIS.
     * WHEN PLAYER ENTERS AREA: CHECK PLAYER'S SPEED, IF GREATER THAN SAD SPEED, AIM TURRET AND KILL PLAYER
     * 
     */

    //VARIABLES FOR CONTROLLING TURRET MOTION
    public float pivotSpeed = 10f;
    public float minClamp = 315;
    public float maxClamp = 45;
    public float currentRotation = 0;

    public bool playerIsColliding = false;

    //last rotation variable used to manage rotation
    // 0 = CounterClockwise, 1 = clockwise
    public float lastRotation = 0;
	// Use this for initialization
	void Start () {
        transform.Rotate(Vector3.forward, pivotSpeed * Time.deltaTime);
        currentRotation = transform.eulerAngles.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (lastRotation == 0 && playerIsColliding == false)
        {
            if (currentRotation < 45 || (0 <= currentRotation && currentRotation >= 315))
            {
                RotateCounterClockwise();
            }
            else if (currentRotation >= 45)
            {
                ChangeDirection();
                lastRotation = 1;
                RotateClockwise();

            }
            else
            {
                Debug.Log("ERROR, TRACKING COUNTERCLOCKWISE ROTATION FAILED");
            }
        }

        if (lastRotation == 1 && playerIsColliding == false)
        {
            if (currentRotation > 315 || ( 0 <= currentRotation && currentRotation <= 45))
            {
                RotateClockwise();
            }
            else if (currentRotation <= 315)
            {
                ChangeDirection();
                lastRotation = 0;
                RotateCounterClockwise();
            }
            else
            {
                Debug.Log("ERROR TRACKING CLOCKWISE ROTATION");
            }
        }
        

	}

    void RotateClockwise()
    {
        transform.Rotate(Vector3.back, pivotSpeed * Time.deltaTime);
        currentRotation = transform.eulerAngles.z;
    }

    void RotateCounterClockwise()
    {
        transform.Rotate(Vector3.forward, pivotSpeed * Time.deltaTime);
        currentRotation = transform.eulerAngles.z;
    }

    void ChangeDirection()
    {
        //ADD SLIGHT PAUSE HERE
    }

}

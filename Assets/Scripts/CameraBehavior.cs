using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour 
{
	//position variables
	public Vector3 offset;

	//camera turn speed variables
	public float xTurnSpeed;
	public float yTurnSpeed;

	//object variables
	public Camera cam;
	public Transform target;
	public Transform camFollower;

	//Lock On Variables
	public bool playerLockedOn;
	public bool targetChanged;
	public Transform enemyToLookAt;
	Quaternion enemyLookAtRot;
	Quaternion turnRot;
	float lockRotInterp;




	// Use this for initialization
	void Start () 
	{
		//sets objects and starting values
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		cam = Camera.main;
		camFollower = GameObject.FindGameObjectWithTag ("Camera Anchor").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//updates camera anchor position to keep up with player position
		transform.position = target.position;

		if (targetChanged) 
		{
			lockRotInterp = 0.0f;
		}

		if (playerLockedOn) 
		{
			if (lockRotInterp >= 1.0f) 
			{
				//transform.LookAt (enemyToLookAt.position);
				transform.rotation = turnRot;
				lockRotInterp = 0.0f;
			} 
			else 
			{
				followLockedOnEnemy ();
			}
			//transform.LookAt (new Vector3 (enemyToLookAt.position.x, 0.0f, enemyToLookAt.position.z));
		} 
		else 
		{
			//calls a function to handle camera rotation input and set pitch limits
			rotateCamera ();
		}

		//updates the camera followers Y rotation to keep track of camera facing direction
		camFollower.transform.rotation = Quaternion.Euler (camFollower.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, camFollower.transform.rotation.eulerAngles.z);

	}

	//handle input from the right joystick, spins camera and sets pitch limits between 5 and 75 degrees
	void rotateCamera()
	{
		//gets input from right joystick
		float horizontalSpin = Input.GetAxis ("Gamepad Camera X");
		float VerticalSpin = Input.GetAxis ("Gamepad Camera Y");


		//if player is moving right joystick on its X axis	
		if (horizontalSpin != 0) 
		{
			//Spins camera anchor and consequently spins player camera around player
			transform.RotateAround(target.position, Vector3.up, horizontalSpin * xTurnSpeed * Time.deltaTime);
		}

		//checks if camera pitch is at or above the upper limit and prevents the camera moving if the player tries to go further up
		if ((transform.rotation.eulerAngles.x >= 75) && (VerticalSpin > 0)) 
		{
			//sets vertical spin to 0 to prevent camera movement if they try to go up
			VerticalSpin = 0;
		}

		//checks if camera pitch is at or below the lower limit and prevents the camera moving if the player tries to go lower
		if ((transform.rotation.eulerAngles.x <= 5) && (VerticalSpin < 0)) 
		{
			//sets vertical spin to 0 to prevent camera movement if they try to go down
			VerticalSpin = 0;
		}

		//checks if there is input on the Y axis of the right joystick
		if (VerticalSpin != 0) 
		{
			//rotates the camera up or down based on right joystick input
			transform.RotateAround(target.position, transform.right, VerticalSpin * yTurnSpeed * Time.deltaTime);
		}
	}

	void followLockedOnEnemy()
	{
		Quaternion forwardLookRot = Quaternion.LookRotation (transform.forward);
		Vector3 directionOfEnemy = enemyToLookAt.position - transform.position;
		enemyLookAtRot = Quaternion.LookRotation (directionOfEnemy);
		turnRot = Quaternion.Euler (forwardLookRot.eulerAngles.x, enemyLookAtRot.eulerAngles.y, enemyLookAtRot.eulerAngles.z);
		//float lockTurnAngle = Vector3.Angle (transform.forward, directionOfEnemy);
		/*if (enemyToLookAt.position.x < transform.right.x) 
		{
			lockTurnAngle *= -1;
		} 
		else 
		{
			lockTurnAngle *= 1;
		}*/

		transform.rotation = Quaternion.Slerp (forwardLookRot, turnRot, lockRotInterp);
		lockRotInterp += Time.deltaTime * 2;
	}
}

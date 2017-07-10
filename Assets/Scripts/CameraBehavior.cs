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
		//calls a function to handle camera rotation input and set pitch limits
		rotateCamera ();
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



}

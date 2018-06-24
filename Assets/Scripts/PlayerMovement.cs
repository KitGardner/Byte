using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    #region Script Variables
    //Speed variables
    [Header("Speed Settings")]
    [SerializeField]
    float _moveSpeed;
	public float moveSpeed;
	public float turnSpeed;

	//movement variables
	private float horizontal;
	private float vertical;
	[Header("Movement Settings")]
	public float downAccel;
	public Vector3 moveDirection;
	private Vector3 velocity;
    public bool isAttacking;

	// jump variables
	[Header("Jump values")]
	public bool isGrounded = true;
	public float jumpStrength;

	//Dodge variables
	[Header("Dodge Values")]
	public float dodgeSpeed;
	public float dodgeWaitTime;
	public bool dodging = false;

	//player rotation variables
	private Quaternion nextRot;
	float rotinterp;
	float turnAngle;

	//object variables
	//public Rigidbody rb;
	[Header("Object References")]
	public Transform cam;
	public CharacterController Char;
	private float charStartHeight;
	private Vector3 charStartPos;
	public PlayerAnimController playerAnim;

	//Melee combat variables
	[Header("Combat Values")]
	public string attackChainList;
	private string lastButton;
	private string curButton;
	public float checkForInputTime;
	//public bool isAttacking = false;
	public bool animationDonePlaying = true;
	#endregion


	// Use this for initialization
	void Start () 
	{
		//initialize values and objects
		transform.rotation = Quaternion.identity;
		cam = GameObject.FindGameObjectWithTag ("Camera Anchor").transform;
		Char = GetComponent<CharacterController> ();
		charStartHeight = Char.height;
		charStartPos = Char.center;
		playerAnim = GetComponent<PlayerAnimController> ();
		animationDonePlaying = true;
		isGrounded = true;
		attackChainList = "";
		lastButton = "";
        isGrounded = true;
	}
	
	// Fixed Update is called every 0.2 seconds
	void FixedUpdate () 
	{
		if (Physics.Raycast (transform.position, Vector3.down, 1.0f)) 
		{
            print("I am hitting ground");
			isGrounded = true;
			playerAnim.setIsGrounded (true);
		} 
		//else 
		//{
		//	isGrounded = false;
		//	playerAnim.setIsGrounded (false);
		//}
		//store the axis values of the controller left joystick
		horizontal = Input.GetAxis ("Gamepad Horizontal");
		vertical = Input.GetAxis ("Gamepad Vertical");

		playerAnim.setSpeedValue (Mathf.Abs(horizontal) + Mathf.Abs(vertical));

		//Calls function that calculates rotation for turning with input
		handleRotation ();

        //Dodge ();
        //checks if the character is dodging and is on the ground
        if (dodging)
        {
            //moves character in direction with new burst of speed
            Char.Move(cam.transform.TransformDirection(moveDirection) * dodgeSpeed * Time.deltaTime);
            //adjusts capsule collider height and center to match mesh 
            Char.center = new Vector3(0f, -0.33f, 0f);
            Char.height = 1.1f;
        }

        if (!(isAttacking))
        {
            //calls function that sets movement values and moves character
            handleMovement();
        }





	}

	#region Movement and Orientation Functions
	//Figures out what player is doing whether they are grounded, jumping, moving, or staying still and modifying movement values. Moves player character
	void handleMovement()
	{
		
		//checks if player is grounded and applies gravity
		if (isGrounded) 
		{
			//player is on the ground and no additional gravity is applied
			moveDirection.y = 0;	
		} 
		else 
		{
			//player is in the air, applies gravity to player
			moveDirection.y -= downAccel;
		}

		moveDirection.x = horizontal * moveSpeed;
		moveDirection.z = vertical * moveSpeed;

		//small delay so player finishes turning before moving
		/*if (rotinterp >= 0.2f) 
		{
			//Sets MoveDirection with joysticks are moved
			moveDirection.x = horizontal * moveSpeed;
			moveDirection.z = vertical * moveSpeed;
		} 
		else 
		{
			//if no input from joysticks, defaults moveDirection
			moveDirection.x = 0;
			moveDirection.z = 0;
		}*/
			
		//checks for jump input
		/*if ((Input.GetButtonDown ("Gamepad Jump")) && (isGrounded)) 
		{
			//calls jump function
			Jump ();
		}*/
			
		//moves character using the camera facing direction as base and applying motion
		Char.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);

	}

	//Figures out degree amount by comparing the x and y axis of the left joystick. Creates a new rotation and spherically interpolates between the current rotation and new rotation
	void handleRotation()
	{

		//calculates amount of degrees by comparing the x and y axis of the left joystick
		if ((horizontal != 0) || (vertical != 0)) 
		{
			turnAngle = Mathf.Atan2 (horizontal, vertical) * (180 / Mathf.PI);
		} 
		else 
		{
			//if no input the interpolating value is reset
			rotinterp = 0;
		}

		//sets the new target rotation by adding the turn angle to the camera rotation
		nextRot = Quaternion.Euler (0,  cam.transform.rotation.eulerAngles.y + turnAngle, 0);
		//rotates player to direction of input using spherical interpolation
		transform.rotation = Quaternion.Slerp (transform.rotation,nextRot, rotinterp);
		//increments the interpolating value in real time
		rotinterp += Time.deltaTime;
	}

    public void faceEnemyWhenAttacking()
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);
        isAttacking = true;
    }
	#endregion
	//handles jumping
	public void Jump()
	{
		//temporarily sets moveDirection for a short jump burst
		moveDirection.y = jumpStrength;
		playerAnim.setJumpTrigger ();
        Char.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);
    }

	#region Dodge Functions
	//Handles dodging. When dodge button is pressed it calls the Dodge Timer Coroutine to create a short burst of speed. Then it moves the character with the new speed and adjusts the capsule collider
	//to match the mesh when rolling
	public void Dodge()
	{
		//checks if the dodge button has been pressed
		/*if (Input.GetButtonDown("Gamepad B") && isGrounded) 
		{
			//starts the Dodge Speed Boost timer
			StartCoroutine ("DodgeTimer");
			//plays dodge animation
			playerAnim.setDodgeTrigger ();
		}*/

        //starts the Dodge Speed Boost timer
        StartCoroutine("DodgeTimer");
        //plays dodge animation
        playerAnim.setDodgeTrigger();

        //checks if the character is dodging and is on the ground
        /*if (dodging) 
		{
			//moves character in direction with new burst of speed
			Char.Move (cam.transform.TransformDirection (moveDirection) * dodgeSpeed * Time.deltaTime);
			//adjusts capsule collider height and center to match mesh 
			Char.center = new Vector3 (0f, -0.33f, 0f);
			Char.height = 1.1f;
		}*/
	}

	//Sets a time limit on the speed boost that comes from dodging
	IEnumerator DodgeTimer ()
	{
		dodging = true;
		//waits as long as dodgeWaittime is set creating the timed burst
		yield return new WaitForSeconds (dodgeWaitTime);
		dodging = false;
		//Resets capsule collider settings
		Char.height = charStartHeight;
		Char.center = charStartPos;
	}
	#endregion

	
}

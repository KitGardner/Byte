using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    
    public float moveSpeed;
    public float gravity;
    public float turnSpeed;
    public float jumpStrength;
    public Transform cam;
    public CharacterController controller;


    private bool buttonHeld;
    private int jumps;
    private Vector3 moveDirection;
    private float horizontal;
    private float vertical;
    private float turnAngle;
    private float rotInterp;
    private Quaternion nextRot;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("Camera Anchor").transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        horizontal = Input.GetAxis("Gamepad Horizontal");
        vertical = Input.GetAxis("Gamepad Vertical");
        HandleRotation();

        if (controller.isGrounded)
        {
            Debug.Log("I am Grounded");

            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            moveDirection.y = gravity;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpStrength;
                print("I am hitting Jump");
            }

            jumps = 0;
        }
        else
        {
            //player is in the air, applies gravity to player
            moveDirection = new Vector3(horizontal, moveDirection.y, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= moveSpeed;
            moveDirection.z *= moveSpeed;
            moveDirection.y -= gravity;

            if (Input.GetButton("Jump") && (jumps < 1))
            {
                moveDirection.y = jumpStrength;
                print("I am hitting Jump in air");
                buttonHeld = true;
                jumps++;
            }

        }

        controller.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);
    }

    //Figures out degree amount by comparing the x and y axis of the left joystick. Creates a new rotation and spherically interpolates between the current rotation and new rotation
    public async Task<bool> HandleRotation()
    {

        //calculates amount of degrees by comparing the x and y axis of the left joystick
        if ((horizontal != 0) || (vertical != 0))
        {
            turnAngle = Mathf.Atan2(horizontal, vertical) * (180 / Mathf.PI);
        }
        else
        {
            //if no input the interpolating value is reset
            rotInterp = 0;
        }

        //sets the new target rotation by adding the turn angle to the camera rotation
        nextRot = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y + turnAngle, 0);
        //rotates player to direction of input using spherical interpolation
        transform.rotation = Quaternion.Slerp(transform.rotation, nextRot, rotInterp);
        //increments the interpolating value in real time
        rotInterp += Time.deltaTime;

        return true;
    }
}

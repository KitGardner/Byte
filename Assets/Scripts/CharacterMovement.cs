using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum playerStates
    {
        freeRoam,
        lockedOn,
        Interacting
    };


    public float moveSpeed;
    public float gravity;
    public float turnSpeed;
    public float jumpStrength;
    public Transform cam;
    public CharacterController controller;
    public InteractiveObject InteractObj;
    public playerStates playerState;


    private bool buttonHeld;
    private bool grounded = false;
    private bool alreadyGrounded = false;
    private int extraJumps;
    private Vector3 moveDirection;
    private float horizontal;
    private float vertical;
    private float turnAngle;
    private float rotInterp;
    private Quaternion nextRot;
    private PlayerAnimController animController;
    private float airTimer = 0f;



    public void Awake()
    {
        InputManager.AxisMessages["Left Joystick"].listeners.Add(HandleMovement);
        InputManager.AxisMessages["Left Joystick"].listeners.Add(HandleRotation);
        InputManager.Inputs["Jump"].listeners.Add(Jump);
    }

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("Camera Anchor").transform;
        animController = GetComponent<PlayerAnimController>();
        playerState = playerStates.freeRoam;
    }


    public async void HandleMovement(float xMove, float yMove)
    {
        grounded |= controller.isGrounded;

        if (controller.isGrounded)
        {
            airTimer = 0;

            Debug.Log("I am Grounded");

            moveDirection = new Vector3(xMove, 0, yMove);
            moveDirection *= moveSpeed;
            moveDirection.y = 0;

            if (!alreadyGrounded)
            {
                extraJumps = 0;
                animController.setIsGrounded(true);
                alreadyGrounded = true;
            }

        }
        else
        {
            airTimer += Time.deltaTime;

            //player is in the air, applies gravity to player
            moveDirection = new Vector3(xMove, moveDirection.y, yMove);
            moveDirection.x *= moveSpeed;
            moveDirection.z *= moveSpeed;

            if (airTimer >= 0.2f)
            {
                grounded = false;
                moveDirection.y -= gravity;
                animController.setIsGrounded(false);
                alreadyGrounded = false;
            }


        }

        controller.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);
        animController.setSpeedValue((Mathf.Abs(xMove) + Mathf.Abs(yMove)));
    }

    //Figures out degree amount by comparing the x and y axis of the left joystick. Creates a new rotation and spherically interpolates between the current rotation and new rotation
    public async void HandleRotation(float xAng, float yAng)
    {

        //calculates amount of degrees by comparing the x and y axis of the left joystick
        if ((xAng != 0) || (yAng != 0))
        {
            turnAngle = Mathf.Atan2(xAng, yAng) * (180 / Mathf.PI);
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

    }

    //GOT IT!! Just had to lock the grounded value as well.
    public void Jump()
    {
        switch (playerState)
        {
            case playerStates.freeRoam:
                {
                    if (grounded)
                    {
                        moveDirection.y = jumpStrength;
                        print("Grounded is Called");
                        controller.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);
                        animController.setJumpTrigger();
                    }
                    else
                    {
                        if (extraJumps < 1)
                        {
                            print("Air is called");
                            moveDirection.y = jumpStrength;
                            extraJumps++;
                            controller.Move(cam.transform.TransformDirection(moveDirection) * Time.deltaTime);
                            animController.setJumpTrigger();
                        }
                    }
                    break;
                }
            case playerStates.lockedOn:
                break;
            case playerStates.Interacting:
                {
                    if (InteractObj != null)
                        InteractObj.Interact();
                    break;
                }
            default:
                break;
        }
        
    }

    public void Dodge()
    {

    }

    public void setPlayerInteraction(bool interacting, InteractiveObject interactingObject)
    {
        InteractObj = interactingObject;

        if (interacting)
            playerState = playerStates.Interacting;
        else
            playerState = playerStates.freeRoam;
    }
}

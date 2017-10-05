using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum playerStates
    {
        freeRoam,
        lockedOn,
        Interacting
    };


    public playerStates playerState;
    public PlayerMovement playerMove;
    public PlayerCombat playerCombat;
    public InteractiveObject InteractObj;
    public bool gamePaused;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Use this for initialization
    
	// Use this for initialization
	void Start ()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerCombat = playerMove.gameObject.GetComponent<PlayerCombat>();
        playerState = playerStates.freeRoam;
        gamePaused = false;	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetButtonDown("Gamepad Jump"))
        {
            switch(playerState)
            {
                case playerStates.lockedOn:
                    playerMove.Dodge();
                    break;
                case playerStates.Interacting:
                    InteractObj.Interact();
                    break;
                case playerStates.freeRoam:
                    if (playerMove.isGrounded)
                        playerMove.Jump();
                    break;
                default:
                    break;
                
            }
               

        }

        checkForAttackInput();

        if(Input.GetButtonDown("Gamepad B"))
        {
            playerCombat.usingLeftArm();
        }

        if(Input.GetButtonDown("Gamepad Start"))
        {
            PauseGame();
        }

	}

    public void setPlayerInteraction(bool interacting, InteractiveObject interactingObject)
    {
        InteractObj = interactingObject;

        if (interacting)
            playerState = playerStates.Interacting;
        else
            playerState = playerStates.freeRoam;
    }

    void PauseGame()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1;
        }
        else
        {
            gamePaused = true;
            Time.timeScale = 0;
        }
    }

    void checkForAttackInput()
    {
        if (Input.GetButtonDown("Gamepad X"))
            playerCombat.usingLightAttack();


        if (Input.GetButtonDown("Gamepad Y"))
            playerCombat.usingHeavyAttack();
    }
}

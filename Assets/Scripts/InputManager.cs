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
    public CharacterMovement playerMove;
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
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        //playerCombat = playerMove.gameObject.GetComponent<PlayerCombat>();
        playerState = playerStates.freeRoam;
        gamePaused = false;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Seems to have about an 80% responsiveness. Perhaps a release build works better?
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
                        playerMove.Jump();
                    break;
                default:
                    break;
                
            }
               

        }

        var horizontal = Input.GetAxis("Gamepad Horizontal");
        var vertical = Input.GetAxis("Gamepad Vertical");

        playerMove.HandleMovement(horizontal, vertical);
        playerMove.HandleRotation(horizontal, vertical);

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

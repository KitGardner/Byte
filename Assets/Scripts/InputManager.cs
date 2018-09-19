using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    //Stores all of the possible inputs and default values. Will be used to store all inputs for the given frame.
    private Dictionary<string, bool> inputMapper = new Dictionary<string, bool>()
    {
        { "JumpButtonPressed", false },
        { "LightAttackButtonPressed", false },
        { "HeavyAttackButtonPressed", false },
        { "SpecialActionButtonPressed", false },
        { "GamepadStartButtonPressed", false},
        { "GamepadRbButtonPressed", false },
        { "GamepadLbButtonPressed", false },
    };

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
        inputMapper["JumpButtonPressed"] |= Input.GetButtonDown("Gamepad Jump");
        inputMapper["LightAttackButtonPressed"] |= Input.GetButtonDown("Gamepad Light Attack");
        inputMapper["HeavyAttackButtonPressed"] |= Input.GetButtonDown("Gamepad Heavy Attack");
        inputMapper["SpecialAttackButtonPressed"] |= Input.GetButtonDown("Gamepad B");
        inputMapper["GamepadStartButtonPressed"] |= Input.GetButtonDown("Gamepad Start");
        inputMapper["GamepadRbButtonPressed"] |= Input.GetButtonDown("Gamepad Weapon Toggle Right");
        inputMapper["GamepadLbButtonPressed"] |= Input.GetButtonDown("Gamepad Weapon Toggle Left");


        HandleInputs(inputMapper);

        //Seems to have about an 80% responsiveness. Perhaps a release build works better?
	    //if (Input.GetButtonDown("Gamepad Jump"))
     //   {
     //       switch(playerState)
     //       {
     //           case playerStates.lockedOn:
     //               playerMove.Dodge();
     //               break;
     //           case playerStates.Interacting:
     //               InteractObj.Interact();
     //               break;
     //           case playerStates.freeRoam:
     //                   playerMove.Jump();
     //               break;
     //           default:
     //               break;
                
     //       }
               

     //   }

     //   var horizontal = Input.GetAxis("Gamepad Horizontal");
     //   var vertical = Input.GetAxis("Gamepad Vertical");

     //   playerMove.HandleMovement(horizontal, vertical);
     //   playerMove.HandleRotation(horizontal, vertical);

     //   checkForAttackInput();

     //   if(Input.GetButtonDown("Gamepad B"))
     //   {
     //       playerCombat.usingLeftArm();
     //   }

     //   if(Input.GetButtonDown("Gamepad Start"))
     //   {
     //       PauseGame();
     //   }

	}

    private void HandleInputs(Dictionary<string, bool> inputs)
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

    private async void HandleInputs()
    {
        var buttonTasks = new List<Task<bool>>();
        foreach (var key in InputMapper.Keys)
        {
            if(InputMapper[key].Value)
            {
                buttonTasks.Add(Inputs[InputMapper[key].Name].SendInputMessage());
            }
        }

        await Task.WhenAll(buttonTasks);

        foreach (var key in InputMapper.Keys)
        {
            InputMapper[key].Value = false;
        }
    }
}

public class InputButtonState
{
    public string Name { get; set; }
    public bool Value { get; set; }

    public InputButtonState(string buttonName)
    {
        this.Name = buttonName;
        this.Value = false;
    }
}

public class InputButtonMessenger
{
    public delegate void messageDel();

    public List<messageDel> listeners;

    public InputButtonMessenger()
    {
        this.listeners = new List<messageDel>();
    }

    public async Task<bool> SendInputMessage()
    {
        if (listeners.Count == 0)
            return true;

        foreach (var listener in listeners)
        {
            listener();
        }

        return true;
    }
}

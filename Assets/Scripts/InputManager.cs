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

    //TODO Create a new dictionary mapping controller buttons to game action descriptions stored in Inputs Dictionary
    //This looks like it is too much. Consider easier way that allows more customized button mapping. Perhaps each value should be a List of InputButtonState objects.
    public Dictionary<string, InputButtonState> InputMapper = new Dictionary<string, InputButtonState>()
    {
        { "A", new InputButtonState("Jump") },
        { "B", new InputButtonState("SpecialAbility") },
        { "X", new InputButtonState("LightAttack") },
        { "Y", new InputButtonState("HeavyAttack") },
        { "Start", new InputButtonState("StartButton") },
        { "RB", new InputButtonState("WeaponToggleRight") },
        { "LB", new InputButtonState("WeaponToggleLeft") },
        { "LT", new InputButtonState("LockOnButton") },
        { "RT", new InputButtonState("ChangeAbilityButton") }
    };

    //Dictionary for storing listeners to each pressed button
    public static Dictionary<string, InputButtonMessenger> Inputs = new Dictionary<string, InputButtonMessenger>()
    {
        {"Jump", new InputButtonMessenger() },
        { "LightAttack", new InputButtonMessenger() },
        { "HeavyAttack", new InputButtonMessenger() },
        { "SpecialAbility", new InputButtonMessenger() },
        { "StartButton", new InputButtonMessenger() },
        { "WeaponToggleLeft", new InputButtonMessenger() },
        { "WeaponToggleRight", new InputButtonMessenger() },
        { "LockOnButton", new InputButtonMessenger() },
        { "ChangeAbilityButton", new InputButtonMessenger() }
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
        //   //Seems to have about an 80% responsiveness. Perhaps a release build works better?
        //if(Input.GetButtonDown("Gamepad Jump"))
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

        InputMapper["A"].Value |= Input.GetButtonDown("Jump");

        HandleInputs();

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

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

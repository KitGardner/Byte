using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public enum playerStates
    //{
    //    freeRoam,
    //    lockedOn,
    //    Interacting
    //};


    //public playerStates playerState;
    public CharacterMovement playerMove;
    public PlayerCombat playerCombat;
    public InteractiveObject InteractObj;
    public bool gamePaused;
    public static bool InputsEnabled = true;

    //Stores all of the possible inputs and default values. Will be used to store all inputs for the given frame.
    private Dictionary<string, InputButtonState> InputMapper = new Dictionary<string, InputButtonState>()
    {
        { "JumpButtonPressed", new InputButtonState("Jump")},
        { "LightAttackButtonPressed", new InputButtonState("LightAttack") },
        { "HeavyAttackButtonPressed", new InputButtonState("HeavyAttack") },
        { "SpecialActionButtonPressed", new InputButtonState("SpecialAbility")},
        { "GamepadStartButtonPressed", new InputButtonState("StartButton")},
        { "GamepadRbButtonPressed", new InputButtonState("WeaponToggleRight") },
        { "GamepadLbButtonPressed", new InputButtonState("WeaponToggleLeft") },
        { "GamepadRtButtonPressed", new InputButtonState("ChangeAbilityButton")}
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

    public static Dictionary<string, InputAxisMessenger> AxisMessages = new Dictionary<string, InputAxisMessenger>()
    {
        { "Right Joystick", new InputAxisMessenger("Camera Rotation")},
        { "Left Joystick", new InputAxisMessenger("Player Movement")},
        { "LT", new InputAxisMessenger("Lock On") }
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
        //playerState = playerStates.freeRoam;
        gamePaused = false;	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (InputsEnabled)
        {
            InputMapper["JumpButtonPressed"].Value |= Input.GetButtonDown("Gamepad Jump");
            InputMapper["LightAttackButtonPressed"].Value |= Input.GetButtonDown("Gamepad X");
            InputMapper["HeavyAttackButtonPressed"].Value |= Input.GetButtonDown("Gamepad Y");
            InputMapper["SpecialActionButtonPressed"].Value |= Input.GetButtonDown("Gamepad B");
            InputMapper["GamepadStartButtonPressed"].Value |= Input.GetButtonDown("Gamepad Start");
            InputMapper["GamepadRbButtonPressed"].Value |= Input.GetButtonDown("Weapon Toggle Right");
            InputMapper["GamepadLbButtonPressed"].Value |= Input.GetButtonDown("Weapon Toggle Left");
            InputMapper["GamepadRtButtonPressed"].Value |= Input.GetButtonDown("Gamepad RT");

            HandleInputs();


            AxisMessages["Right Joystick"].XValue = Input.GetAxis("Gamepad Camera X");
            AxisMessages["Right Joystick"].YValue = Input.GetAxis("Gamepad Camera Y");
            AxisMessages["Left Joystick"].XValue = Input.GetAxis("Gamepad Horizontal");
            AxisMessages["Left Joystick"].YValue = Input.GetAxis("Gamepad Vertical");
            AxisMessages["LT"].XValue = Input.GetAxis("Gamepad LT");

            HandleAxis();
        }

     //   if(Input.GetButtonDown("Gamepad Start"))
     //   {
     //       PauseGame();
     //   }

	}

    //public void setPlayerInteraction(bool interacting, InteractiveObject interactingObject)
    //{
    //    InteractObj = interactingObject;

    //    if (interacting)
    //        playerState = playerStates.Interacting;
    //    else
    //        playerState = playerStates.freeRoam;
    //}

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

    private async void HandleAxis()
    {
        foreach (var key in AxisMessages.Keys)
        {
            float xVal = AxisMessages[key].XValue;
            float yVal = AxisMessages[key].YValue;
            AxisMessages[key].SendInputMessage(xVal, yVal);           
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

public class InputAxisState
{
    public string Name { get; set; }
    public float XValue { get; set; }
    public float YValue { get; set; }

    public InputAxisState(string buttonName)
    {
        this.Name = buttonName;
        this.XValue = 0f;
        this.YValue = 0f;
    }
}

public class InputAxisMessenger
{
    public delegate void messageDel(float horizontal, float vertical = 0f);

    public List<messageDel> listeners;

    public string Name { get; set; }
    public float XValue { get; set; }
    public float YValue { get; set; }

    public InputAxisMessenger(string Name)
    {
        this.listeners = new List<messageDel>();
        this.Name = Name;
        this.XValue = 0f;
        this.YValue = 0f;
    }

    public async void SendInputMessage(float horizontal, float vertical)
    {
        if (listeners.Count == 0)
            return;

        foreach (var listener in listeners)
        {
            listener(horizontal, vertical);
        }
    }

    public async void SendInputMessage(float Axis)
    {
        if (listeners.Count == 0)
            return;

        foreach (var listener in listeners)
        {
            listener(Axis);
        }
    }
}

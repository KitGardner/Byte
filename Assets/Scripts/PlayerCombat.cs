using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour 
{
	//Lock On Variables
	bool targetLocked;
	bool targetChanged;
	public GameObject lockedOnEnemy;
	public GameObject lockOnParticle;
	private GameObject lockOnInstance;
	public List<GameObject> enemiesWithinLockRange;

    //Melee Combat variables
    public bool isAttacking;
    public bool canSwitch;
    public float attackTimer;
    public float maxAttackTime;
    public WeaponHitScript hitBoxScript;
    public bool usingGreatsword;
    public int lightAttackCount;

	//Object references
	public CameraBehavior camController;
	public Transform cam;
    public PlayerAnimController playerAnim;
    public PlayerStats playerStats;
    public PlayerMovement playerMove;
    public InputManager inputManager;
    public Transform leftArmAbilityAnchor;

    //Left Arm Object Values
    public GameObject thrownStakes;
    private ThrownStakeHit thrownStakeScript;

	//Special class for store and retrieving combat animations
	/*public class AttackAnimation
	{
		string normLightAttack;
		string normHeavyAttack;
		string transitionLightAttack;
		string transitionHeavyAttack;

		public AttackAnimation(string normXAttack, string normYAttack, string tranXAttack, string tranYAttack)
		{
			normLightAttack = normXAttack;
			normHeavyAttack = normYAttack;
			transitionLightAttack = tranXAttack;
			transitionHeavyAttack = tranYAttack;
		}

		public string GetAttackAnimation(string pressedButton)
		{
			if (pressedButton == "X") {return normLightAttack;} 
			else if (pressedButton == "Y") {return normHeavyAttack;}

			return "No attack available";
		}

		public string GetAttackAnimation(string pressedButton, bool transitioning)
		{
			if ((pressedButton == "X") && transitioning) {return transitionLightAttack;} 
			else if ((pressedButton == "Y") && transitioning) {return transitionHeavyAttack;}

			return "No attack available";
		}
	
	}*/

    

	// Use this for initialization
	void Start () 
	{
		//Initialize all object references and starting values. Add attack animations to list
		cam = GameObject.FindGameObjectWithTag ("Camera Anchor").transform;
		camController = GameObject.FindGameObjectWithTag ("Camera Controller").GetComponent<CameraBehavior> ();
        playerAnim = GetComponent<PlayerAnimController>();
        playerStats = GetComponent<PlayerStats>();
        playerMove = GetComponent<PlayerMovement>();
        inputManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InputManager>();
        leftArmAbilityAnchor = GameObject.FindGameObjectWithTag("LeftArmAnchor").transform;
		//chainAttacks.Add (attackOne);
		//chainAttacks.Add (attackTwo);
		//chainAttacks.Add (attackThree);
		//chainAttacks.Add (attackFour);
	}
	
	// Update is called once per frame
	void Update () 
	{
        #region Lock On Section
        //if LT is pressed. Runs every frame LT is held
        if (Input.GetAxis ("Gamepad LT") > 0.0f)
		{
            //print("LT is being pressed");
			//checks if there are any enemies within lock on range
			if (!(enemiesWithinLockRange.Count <= 0)) 
			{
				//checks if the player is already locking on. Breaks if statements if the player is already locked on
				if (!targetLocked) 
				{
					//calls function to retrieve enemy that is nearest to camera line of sight, locks on to enemy.
					lockedOnEnemy = getLockOnTarget ();
					targetLocked = true;
					//sets camera to look at locked on enemy
					camController.playerLockedOn = true;
					camController.enemyToLookAt = lockedOnEnemy.transform;
                    inputManager.playerState = InputManager.playerStates.lockedOn;

					//spawns particle effect to show what enemy is locked on. Will be switched later with proper graphic
					lockOnInstance = Instantiate (lockOnParticle);
				}
			}
		}

		//checks if the player is locked on to an enemy
		if (targetLocked) 
		{
			//Sets the lock on graphic to the position of the locked on enemy
			lockOnInstance.transform.position = lockedOnEnemy.transform.position;

			//Checks for when the player releases LT
			if ((Input.GetAxis ("Gamepad LT") <= 0.0f) || (enemiesWithinLockRange.Count <= 0)) 
			{
				//disables lock on
				lockedOnEnemy = null;
				targetLocked = false;
                inputManager.playerState = InputManager.playerStates.freeRoam;

                //sets camera to normal behaviour
                camController.playerLockedOn = false;
				camController.enemyToLookAt = null;

				//destroys lock on graphic
				Destroy (lockOnInstance);
			}

			//keeps track of right joystick horizontal direction
			float rightStickDirection = Input.GetAxis ("Gamepad Camera X");

			//if the right joystick is pressed to the right
			if (rightStickDirection > 0.1) 
			{
				//checks that the target has not already been changed for this instance of pushing the joystick.
				if (!targetChanged) 
				{
					//calls function to get the next target on the right of the camera's facing direction
					lockedOnEnemy = getNextTarget (true);
					targetChanged = true;

					//sets the camera to look at the new target
					camController.enemyToLookAt = lockedOnEnemy.transform;
					camController.targetChanged = true;
				}

			}
			//checks if the right joystick is pressed to the left
			else if (rightStickDirection < -0.1) 
			{
				//checks that the target has not already been changed for this instance of pushing the joystick
				if (!targetChanged) 
				{
					//calls function to get the next target on the left of the camera's facinmg direction
					lockedOnEnemy = getNextTarget (false);
					targetChanged = true;

					//sets the camrea to look at the new target
					camController.enemyToLookAt = lockedOnEnemy.transform;
					camController.targetChanged = true;
				}
			
			}

			//if the joystick is not pushed. Reset the target changed variable for both the player and camera so that they are ready for the next target change call
			else 
			{
				targetChanged = false;
				camController.targetChanged = false;
			}

		}
        #endregion

        #region Player Combat
        if (playerAnim.anim.applyRootMotion == true)
        {
            if (playerAnim.anim.GetNextAnimatorStateInfo(1).IsName("Attack Layer.Attack Base State"))
            {
                playerAnim.anim.applyRootMotion = false;
                isAttacking = false;
                usingGreatsword = false;
                playerMove.isAttacking = false;
                lightAttackCount = 0;
                playerStats.canSwitchWeapons = true;
                playerStats.unequipWeapon();
                playerAnim.isAttacking(isAttacking, playerStats.weaponName);
                playerAnim.usingLeftArm(false);
            }
        }

        /*if(Input.GetButtonDown("Gamepad X"))
        {
            playerAnim.anim.applyRootMotion = true;
            isAttacking = true;
            playerMove.faceEnemyWhenAttacking();
            if (!(playerStats.weaponEquipped))
                playerStats.placeWeaponInHand();
            playerStats.canSwitchWeapons = false;

            playerAnim.isAttacking(isAttacking, playerStats.weaponName);
            playerAnim.lightAttack();
        }

        if (Input.GetButtonDown("Gamepad Y"))
        {
            playerAnim.anim.applyRootMotion = true;
            isAttacking = true;
            playerMove.faceEnemyWhenAttacking();
            if (!(playerStats.weaponEquipped))
                playerStats.placeWeaponInHand();
            playerStats.canSwitchWeapons = false;

            playerAnim.isAttacking(isAttacking, playerStats.weaponName);
            playerAnim.heavyAttack();
        }*/

    }

    public void turnDamageOn()
    {
        hitBoxScript.turnDamageOn();
    }

    public void turnDamageOff()
    {
        hitBoxScript.turnDamageOff();
    }
    #endregion

    //Handles checking if anything is within the sight sphere collider attached to player
    void OnTriggerEnter(Collider other)
	{
		//checks if the colliding object is an enemy
		if (other.tag == "Enemy") 
		{
			//object is an enemy and is added to the list of enemies within lock on range
			enemiesWithinLockRange.Add (other.gameObject);
		}	
	}

	//Handles checking if anything has left the sight sphere collider attached to player
	void OnTriggerExit(Collider other)
	{
		//checks if the leaving object is an enemy
		if (other.tag == "Enemy") 
		{
			//leaving object is an enemy, remove from list of enemies within lock on range
			enemiesWithinLockRange.Remove (other.gameObject);
		}
	}

	//Is called when player presses LT. Examines list of enemies within lock on range. Compares the angle from camera facing direction to direction of each enemy. Returns the enemy that is closest to the
	//cameras line of sight
	GameObject getLockOnTarget()
	{
		//lastLockOnAngle to used to store the angle of the closest enemy is initially set to 190.0 when the function is called
		float lastLockOnAngle = 190.0f;

		//enemy gameobject that will be returned to the function caller. Initially set to null
		GameObject closestEnemy = null;

		//searches the whole list to find which enemy is closests to line of sight
		for (int i = 0; i < enemiesWithinLockRange.Count; i++) 
		{
			//determines angle enemy is at based off enemy location and camera facing direction
			Vector3 lockOnDirection = enemiesWithinLockRange [i].transform.position - transform.position;
			float lockOnAngle = Vector3.Angle (cam.transform.forward, lockOnDirection);

			//is the angle between this enemy and the camera facing direction less than the recorded angle?
			if (lockOnAngle < lastLockOnAngle) 
			{
				//sets the new angle as the last lock on angle for continued testing
				lastLockOnAngle = lockOnAngle;
				//records which enemy is the closest to the camera line of sight
				closestEnemy = enemiesWithinLockRange[i];
			}
		}

		//returns the enemy that is closest to the camera's line of sight
		return closestEnemy;
	}

	//This function searches the list of enemies with lock on range to find out which on is closest on the left and right, and which enemy is the farthest
	//returns the enemy game object based off the input. If switchingToRight is true it returns the closest enemy on the right.
	//If switchingToRight is false it returns the closest enemy on the left
	//If there is no enemy at the indicated direction it insteads returns the farthest enemy in the opposite direction completing the loop
	GameObject getNextTarget(bool switchingToRight)
	{
		//Initializes the variables used to determine and store the closest left and right enemy as well as the farthest enemy from the camera's facing direction
		int closestLeftEnemyIndex = 0;
		float closestLeftEnemyAngle = 190.0f;
		int closestRightEnemyIndex = 0;
		float closestRightEnemyAngle = 190.0f;
		int farthestEnemyIndex = 0;
		float fartherEnemyAngle = 0.0f;

		//searchs the list recording what enemy is the closest to the right and left and what enemy is the farthest
		for (int i = 0; i < enemiesWithinLockRange.Count; i++) 
		{	
			//if this gameobject is the one that the player is currently looking at; finish the loop and iterate to the next index
			if (enemiesWithinLockRange [i] == lockedOnEnemy) 
			{
				continue;
			}
			//this object is a different enemy. Check if it is to the right or left and determine if it is closer than the previous enemy. Lastly, see if it is the farthest enemy from the camera's view
			else 
			{
				//determines the relative direction, angle, and position of this enemy from the camera's facing direction
				Vector3 relativeDirection = enemiesWithinLockRange [i].transform.position - transform.position;
				Vector3 relativePos = cam.transform.InverseTransformPoint (enemiesWithinLockRange [i].transform.position);
				float relativeAngle = Vector3.Angle (cam.transform.forward, relativeDirection);

				//if the enemy is to the left of the camera facing direction and is closer than the other left enemies
				if ((relativePos.x < 0.0f) && (relativeAngle < closestLeftEnemyAngle)) 
				{
					//set this enemy to the closest left enemy
					closestLeftEnemyAngle = relativeAngle;
					closestLeftEnemyIndex = i;
				}
				//if the enemy is to the right of the camera facing direction and is closer that the other right enemies
				else if((relativePos.x > 0.0f) && (relativeAngle < closestRightEnemyAngle))
				{
					//set this enemy to the closest right enemy
					closestRightEnemyAngle = relativeAngle;
					closestRightEnemyIndex = i;
				}

				//if this enemy is farther that the last recorded enemy
				if (relativeAngle > fartherEnemyAngle) 
				{
					//set this enemy as the farthest enemy
					fartherEnemyAngle = relativeAngle;
					farthestEnemyIndex = i;
				}
			}

		}



		//if the player wants to switch to the enemy on the right
		if (switchingToRight) 
		{
			//if there is an enemy to the right of the camera's facing direction
			if (!(closestRightEnemyAngle == 190.0f)) 
			{
				//return closest enemy on the right
				return enemiesWithinLockRange [closestRightEnemyIndex];
			} 
		}
		//if the playter wants to switch to the enemy on the left
		else if (!(switchingToRight)) 
		{
			//if there is an enemy to the left of the camera's facing direction
			if (!(closestLeftEnemyAngle == 190.0f)) 
			{
				//return closest enemy on the left
				return enemiesWithinLockRange [closestLeftEnemyIndex];
			} 
		} 

		//if there is no enemy in the direction that the player picks. Return the enemy that is farthest away from the player
		return enemiesWithinLockRange [farthestEnemyIndex];
	}

    public void usingLightAttack()
    {
        playerAnim.anim.applyRootMotion = true;
        isAttacking = true;
        playerMove.faceEnemyWhenAttacking();
        if (!(playerStats.weaponEquipped))
            playerStats.placeWeaponInHand();
        playerStats.canSwitchWeapons = false;

        playerAnim.isAttacking(isAttacking, playerStats.weaponName);
        playerAnim.lightAttack();
    }

    public void usingHeavyAttack()
    {
        playerAnim.anim.applyRootMotion = true;
        isAttacking = true;
        playerMove.faceEnemyWhenAttacking();
        if (!(playerStats.weaponEquipped))
            playerStats.placeWeaponInHand();
        playerStats.canSwitchWeapons = false;

        playerAnim.isAttacking(isAttacking, playerStats.weaponName);
        playerAnim.heavyAttack();
    }

    public void usingLeftArm()
    {
        playerAnim.anim.applyRootMotion = true;
        playerMove.faceEnemyWhenAttacking();
        playerAnim.usingLeftArm(true);

        if(playerStats.curArmSkill == "Stakes")
                playerAnim.throwStake();
    }

    public void createStake()
    {
        thrownStakeScript = Instantiate(thrownStakes, leftArmAbilityAnchor.position, Quaternion.Euler(new Vector3(90, transform.rotation.eulerAngles.y, 0))).GetComponent<ThrownStakeHit>();
        thrownStakeScript.setTrajectory(transform.forward, 2000f);   
    }
}

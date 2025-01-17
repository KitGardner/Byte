﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour 
{
	//health stats
	public float maxHealth;
	public float health;

	//virus stats
	public float maxVirusAmt;
	public float virusAmt;
	private float virusStallMaxTime;
	private float virusCurStallTime;

	//Left Arm stats
	public string[] listOfArmSkills;
	public string curArmSkill;
	private int skillIndex;
	public float maxCharge;
	public float curCharge;
	bool oldTriggerHeld = false;
	public HUDManager hudManager;

	//checks if player is dead
	bool isDead = false;

	//Material Variables
	//public Material baseMaterial;
	//public Material sickMaterial;
	//public Material charMaterial;

	//Equipped weapon variables
	public GameObject[] listOfWeapons;
    public GameObject[] heldWeapons;
	public GameObject equippedWeapon;
    public GameObject weaponInHand;
    public bool weaponEquipped;
	public int itemIndex;
	public Transform weaponAttachmentPoint;
    public string weaponName;
    public bool canSwitchWeapons;

	private PlayerAnimController playerAnim;

	// Use this for initialization
	void Start () 
	{
		//initializes the starting weapon point and sets the first weapon active
		itemIndex = 0;
		skillIndex = 0;
		equippedWeapon = getWeaponAtIndex (itemIndex);
		weaponAttachmentPoint = GameObject.FindGameObjectWithTag ("Weapon Attach").transform;
		listOfWeapons [itemIndex].SetActive (true);
        heldWeapons[itemIndex].SetActive(false);
		curArmSkill = getAbilityatIndex (skillIndex);
		//charMaterial = GameObject.FindGameObjectWithTag ("Material Tester").GetComponent<Renderer>().material;
		hudManager = GameObject.FindGameObjectWithTag ("Game HUD").GetComponent<HUDManager> ();
		hudManager.setAbilityIcon (skillIndex);
		playerAnim = GetComponentInChildren<PlayerAnimController>();
		virusCurStallTime = 0.0f;
		virusStallMaxTime = 0.0f;
        canSwitchWeapons = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//checks if the player is dead
		if (!isDead) 
		{

			if (!(virusCurStallTime < virusStallMaxTime)) {
				//deducts from the virus total in real time divided by 2
				adjVirusAmt (-Time.deltaTime / 2);
			} 
			else 
			{
				virusCurStallTime += Time.deltaTime;
			}

			adjLeftArmCharge (-Time.deltaTime);

			//When the player hits RB it will disable the current weapon and enable the next weapon going up in the array
			if (Input.GetButtonDown ("Weapon Toggle Right")) 
			{
                if (canSwitchWeapons)
                {
                    //Turns off current weapon
                    equippedWeapon.SetActive(false);
                    //Adds 1 to the item index and returns the new weapon
                    equippedWeapon = increaseWeaponIndex();
                    //sets the new weapon to active
                    equippedWeapon.SetActive(true);
                }
			}

			//When the player hits LB it will disable the current weapon and enable the next weapon going down in the array
			else if (Input.GetButtonDown ("Weapon Toggle Left")) 
			{
                if (canSwitchWeapons)
                {
                    //Turns off current weapon
                    equippedWeapon.SetActive(false);
                    //Subtracts 1 from the item index and returns the new weapon
                    equippedWeapon = decreaseWeaponIndex();
                    //Sets the new weapon to active
                    equippedWeapon.SetActive(true);
                }
			}

			//stores the state of the trigger press as a bool
			bool newTriggerHeld = Input.GetAxis ("Gamepad RT") > 0;

			//checks if the flipped old trigger state and the new trigger state are both true. As this cycles the enclosed code will only work if the button was unpressed and then pressed
			//holding the button will do nothing and must be released before the cycle can restart
			if (!oldTriggerHeld && newTriggerHeld) 
			{
				//sets the current arm skills to the next skill in the array
				curArmSkill = getNextSkill ();
			}
			//sets the old trigger state equal to the new one. This keeps up with whether the button is still being held or pressed and released
			oldTriggerHeld = newTriggerHeld;
		}

	}
		

	//add or subtract from health
	public void adjHealth(float adj)
	{
		//adjust health with incoming value
		health += adj;

		//if the adjusted health is more than the maximum set the health equal to maximum
		if (health > maxHealth)
			health = maxHealth;


		//checks if player health is 0
		if (health <= 0) 
		{
			//if the value drops below 0 set it to 0
			health = 0; 
			//call death function
			Die ();
		}
	}

	//divides health by Maxhealth and returns a float between 0 and 1
	public float getHealthRatio()
	{
		//returns a decimal by dividing health by maxHealth
		return health / maxHealth;
	}

	//increases player maxHealth
	public void increaseMaxHealth(float increase)
	{
		//increases maxHealth by incoming value
		maxHealth += increase;
	}

	// destroys player and ends game
	private void Die()
	{
		isDead = true;
		playerAnim.setIsDead ();
		Application.Quit ();
	}
		
	//add or subtract from virusAmt
	public void adjVirusAmt(float adj)
	{
		//when the virus hits 0 start taking from health instead
		if (virusAmt <= 0) 
		{
			//if the virus amount is below 0 set it to 0
			virusAmt = 0;
			//calls adjHealth function to deduct from health in real time
			adjHealth (-Time.deltaTime);
		} 
		else 
		{
			//if there is virus left this adjust the amount
			virusAmt += adj;
		}

		//if the adjusted virus amount is more than the maximum set the virus amount to the maximum
		if (virusAmt > maxVirusAmt)
			virusAmt = maxVirusAmt;

		//changePlayerMaterial ();
	}

	//divides virusAmt by maxVirusAmt and returns a float between 0 and 1
	public float getVirusRatio()
	{
		//returns a decimal by dividing virus amount by maximum virus amount
		return virusAmt / maxVirusAmt;
	}

	//Takes in a float and sets the virus stall time to it. Also resets the virus cur time to start timer.
	public void stallVirus(float stallTime)
	{
		virusStallMaxTime = stallTime;
		virusCurStallTime = 0.0f;
	}

	//increases player maxVirusAmt
	public void increaseMaxVirusAmt(float increase)
	{
		//increases max virus amount with incoming value
		maxVirusAmt += increase;
	}

	//Changes player material base off virus level
	//void changePlayerMaterial()
	//{
	//	//presently this changes the material of the capsule by using lerp. When the proper player character and material are added this will change to the player Material
	//	charMaterial.Lerp (sickMaterial, baseMaterial, getVirusRatio ());
	//}

	//adjusts the charge amount of the left arm with incoming value
	public void adjLeftArmCharge(float adj)
	{
		//adjust current charge with incoming value
		curCharge += adj;
	}

	//divides current charge by max charge and returns a float between 0 and 1
	public float getLeftArmChargeRatio()
	{
		//returns a decimal by dividing current charge amount by max charge
		return curCharge / maxCharge;
	}

	//increases player left arm max charge
	public void increaseLeftArmMaxCharge(float increase)
	{
		//increases max charge with incoming values
		maxCharge += increase;
	}

	//Returns the weapon at the incoming index
	public GameObject getWeaponAtIndex(int i)
	{
        weaponName = listOfWeapons[i].name;
        print(weaponName);
		return listOfWeapons [i];
	}

	//Increases Item index by 1 and returns the next weapon in the array. Also checks if the new item index number will go above the range and resets it
	public GameObject increaseWeaponIndex()
	{
		//increases the item index value
		itemIndex++;

		//checks if the new item index is more than the index numbers of the array
		if (itemIndex > listOfWeapons.Length - 1)
			//if the new item index number is higher it is reset to 0 to close the loop
			itemIndex = 0;
		//returns the weapon at item Index
		return getWeaponAtIndex (itemIndex);
	}

	//Decreases Item index by 1 and returns the next weapon in the array. Also checks if the new item index is less than 0 and sets it each to the array length - 1 to close the loop
	public GameObject decreaseWeaponIndex()
	{
		//decreases the item index value
		itemIndex--;

		//checks if the new item index is below 0
		if (itemIndex < 0)
			//if the new item index is below 0 it is set to the last index of the array, closing the loop
			itemIndex = listOfWeapons.Length - 1;
		//returns the weapon at item index
		return getWeaponAtIndex (itemIndex);
	}

    public void placeWeaponInHand()
    {
        listOfWeapons[itemIndex].SetActive(false);
        heldWeapons[itemIndex].SetActive(true);
        weaponInHand = heldWeapons[itemIndex];
        weaponEquipped = true;
    }

    public void unequipWeapon()
    {
        heldWeapons[itemIndex].SetActive(false);
        listOfWeapons[itemIndex].SetActive(true);
        weaponInHand = null;
        weaponEquipped = false;
    }

	//get the arm ability stored at the index of the array
	public string getAbilityatIndex(int i)
	{
		//returns the ability stored at index
		return listOfArmSkills [i];
	}

	//increases the skill index value and returns the arm ability at the new index. Also check is the index is going out of range and resets it
	public string getNextSkill()
	{
		//increase skill index
		skillIndex++;


		//checks if skill index is out of range and sets it to zero if it is
		if (skillIndex > listOfArmSkills.Length - 1)
			skillIndex = 0;

		hudManager.setAbilityIcon (skillIndex);

		//return the arm ability at the new skill index
		return getAbilityatIndex (skillIndex);
	}
}

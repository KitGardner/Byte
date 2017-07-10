using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour 
{
	public class AttackAnimation
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
			print ("Loaded animations");
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
	}

	private AttackAnimation attackOne;
	private AttackAnimation attackTwo;
	private AttackAnimation attackThree;
	private AttackAnimation attackFour;
	private List<AttackAnimation> chainAttacks;

	// Use this for initialization
	void Start () 
	{
		attackOne = new AttackAnimation ("Forward Slash", "Slam", "Whirlwind", "Uppercut");
		attackTwo = new AttackAnimation ("Backhand", "Somersault", "Thrust", "Smash");
		attackThree = new AttackAnimation ("Stab", "Uplift", "Clim Hazzard", "Charge");
		attackFour = new AttackAnimation ("Pull Away", "Batter Swing", "Down Thrust", "Falling Slash");
		chainAttacks = new List<AttackAnimation>();
		chainAttacks.Add (attackOne);
		chainAttacks.Add (attackTwo);
		chainAttacks.Add (attackThree);
		chainAttacks.Add (attackFour);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmFuelScript : MonoBehaviour 
{
	public float fuelAmount;
	public PlayerStats playerStats;
	// Use this for initialization
	void Start () 
	{
		playerStats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") && !(playerStats.getLeftArmChargeRatio() >= 1.0f)) 
		{
			playerStats.adjLeftArmCharge (fuelAmount);
			Destroy (this.gameObject);
		}
	}
}

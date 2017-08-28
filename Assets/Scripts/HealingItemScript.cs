using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItemScript : MonoBehaviour 
{

	public float healingAmount;
	public PlayerStats playerStats;

	void Start()
	{
		playerStats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if ((other.tag == "Player") && !(playerStats.getHealthRatio() >= 1.0f)) 
		{
			playerStats.adjHealth (healingAmount);
			Destroy (this.gameObject);
		}
	}
}

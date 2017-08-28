using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterScript : MonoBehaviour 
{
	public float virusStallTime;
	public PlayerStats playerStats;

	// Use this for initialization
	void Start () 
	{
		playerStats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			playerStats.stallVirus (virusStallTime);
			Destroy (this.gameObject);
		}
	}
}

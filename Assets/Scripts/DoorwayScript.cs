using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayScript : MonoBehaviour 
{
	public string levelToLoad;
	public LevelManager levelManager;
	public bool playerAtThreshold;

	// Use this for initialization
	void Start () 
	{
		levelManager = GameObject.Find ("Scene Manager").GetComponent<LevelManager>();
		playerAtThreshold = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerAtThreshold) 
		{
			if(Input.GetButtonDown("Gamepad Jump"))
			{
				levelManager.LoadScene (levelToLoad);
			}
		}

		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			playerAtThreshold = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			playerAtThreshold = false;
		}
	}
}

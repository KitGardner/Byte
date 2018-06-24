
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorwayScript : InteractiveObject
{
	public string levelToLoad;
	public LevelManager levelManager;
	public bool playerAtThreshold;
    public GameObject DoorLockedParticle;

    // Use this for initialization
    void Start () 
	{
		levelManager = GameObject.Find ("Scene Manager").GetComponent<LevelManager>();
        inputManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InputManager>();
		playerAtThreshold = false;
	}
	
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
            if (DuvallEstateExtManager.ManorDoorUnlocked)
            {
                playerAtThreshold = true;
                setPlayerInteractivity(playerAtThreshold);
            }
            else
            {
                Instantiate(DoorLockedParticle, this.transform.position + new Vector3(0, 0, 1), Quaternion.identity);
            }


		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			playerAtThreshold = false;
            setPlayerInteractivity(playerAtThreshold);
		}
	}

    

    public override void Interact()
    {
        levelManager.LoadScene(levelToLoad);
    }
}

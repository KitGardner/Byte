
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
    private GameObject tempLockedParticle;
    private CharacterMovement charMove;
    private BoxCollider collider;

    // Use this for initialization
    void Start () 
	{
		levelManager = GameObject.Find ("Scene Manager").GetComponent<LevelManager>();
        inputManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<InputManager>();
        collider = GetComponent<BoxCollider>();
		playerAtThreshold = false;
	}
	
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
            charMove = col.GetComponent<CharacterMovement>();
            playerAtThreshold = true;
            setPlayerInteractivity(playerAtThreshold, charMove, this);
        }
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			playerAtThreshold = false;
            setPlayerInteractivity(playerAtThreshold, charMove, null);
            charMove = null;
		}
	}

    

    public override void Interact()
    {
        if (DuvallEstateExtManager.ManorDoorUnlocked)
            levelManager.LoadScene(levelToLoad);
        else
        {
            if (tempLockedParticle == null)
            {
                tempLockedParticle = Instantiate(DoorLockedParticle, this.transform.position + new Vector3(0, -8, 1), Quaternion.identity);
                //this.collider.enabled = false;
            }
        }
    }
}

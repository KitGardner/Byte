using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : InteractiveObject
{
    private CharacterMovement charMove;
    private bool playerAtThreshold;
    public override void Interact()
    {
        //Once the saving scheme has been decided I will be adding code to active the saving system.
        print("Save Circle Activated!!!!");
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        print("Entered");
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
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Duvall_Exterior_ManorDoor_Cutscene : Cinematic 
{
    public CharacterMovement playerMove;
    public Transform startPos;
    public Transform endPos;
    public Camera cinematicCamera;
    public DoorwayScript door;


    private string name = "Duvall Exterior Manor Door Cinematic";
    public override string CinematicName
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public override async Task<bool> PlayCinematic()
    {
        playerMove.transform.position = startPos.position;
        playerMove.transform.rotation = startPos.rotation;

       return await WaitForReaction();

    }

    public async Task<bool> WaitForReaction()
    {
        System.Threading.Thread.Sleep(5000);
        door.SpawnBarrier();
        var controller = playerMove.GetComponent<CharacterController>();
        //controller.attachedRigidbody.AddExplosionForce(1000, door.transform.position, 100);
        controller.Move(new Vector3(0, 50, 50) * Time.deltaTime);
        return true;
    }

    // Use this for initialization
    void Start () { 
		
	}


	
	// Update is called once per frame
	void Update () {
		
	}

    public override void PlayTest()
    {
        door.SpawnBarrier();
        var controller = playerMove.GetComponent<CharacterController>();
        //controller.attachedRigidbody.AddExplosionForce(1000, door.transform.position, 100);
        controller.Move(new Vector3(0, 10, 10));
    }
}

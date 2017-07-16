using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour 
{

	public Animator anim;

	private float speed;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		anim.SetFloat ("Speed", speed);
	}

	public void setSpeedValue(float value)
	{
		speed = value;
	}

	public void setJumpTrigger()
	{
		anim.SetTrigger ("Jump");
	}

	public void setDodgeTrigger()
	{
		anim.SetTrigger ("Dodging");
	}

	public void setIsGrounded(bool groundState)
	{
		anim.SetBool ("IsGrounded", groundState);
	}

	public void setIsDead()
	{
		anim.SetTrigger ("IsDead");
	}
}

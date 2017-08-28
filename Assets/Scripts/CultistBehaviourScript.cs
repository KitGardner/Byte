using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CultistBehaviourScript : EnemyAIBase 
{

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		enemyCapCol = GetComponent<CapsuleCollider> ();
		enemySphere = GetComponent<SphereCollider> ();
		aiNavAgent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerDetected) 
		{
			moveToLocation ();
		}	
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isDead) 
		{
			checkForPlayer (other);
		}
	}

	void OnTriggerExit(Collider other)
	{
		playerDetected = false;
		anim.SetFloat ("Speed", 0.0f);
		rotInterp = 0.0f;
	}
}

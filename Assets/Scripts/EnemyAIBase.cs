using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyAIBase : MonoBehaviour 
{

	//Enemy sight variables
	public float fieldOfView;
	public Vector3 direction;
	public GameObject player;
	public bool playerDetected;

	//movement variables
	public float gravity;
	public float moveSpeed;
	public float rotInterp;
	public float playerDistToSelf;
	public Vector3 origin;
	private Quaternion startRotation;
	private float yOriginOffset;
	public float maxDistanceFromOriginAllowed;
	public float curDistanceFromOrigin;
	private bool reachedEndOfLeash;

	//component references
	public SphereCollider enemySphere;
	public Animator anim;
	public NavMeshAgent aiNavAgent;
	public CapsuleCollider enemyCapCol;

	//Monster stat values
	public float health;
	public float maxHealth;
	public bool isDead;
	public float despawnTimer;
	public float damage;


	// Use this for initialization
	void Start () 
	{
		initializeMonster ();
	}

	public void initializeMonster()
	{
		enemySphere = GetComponent<SphereCollider> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator> ();
		aiNavAgent = GetComponent<NavMeshAgent> ();
		aiNavAgent.speed = moveSpeed;
		isDead = false;
		enemyCapCol = GetComponent<CapsuleCollider> ();
		origin = transform.position;
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerDetected) 
		{
			moveToLocation ();
		}

		curDistanceFromOrigin = Vector3.Distance (transform.position, origin);
		yOriginOffset = curDistanceFromOrigin;

		if (curDistanceFromOrigin >= maxDistanceFromOriginAllowed) 
		{
			playerDetected = false;
			reachedEndOfLeash = true;
			rotInterp = 0.0f;
			updateAnimation ("IsBackingUp", true);
		}

		if (reachedEndOfLeash) 
		{
			returnToOrigin ();
		}

		/*else 
		{
			despawnTimer -= Time.deltaTime;

			if (despawnTimer <= 0.0f) 
			{
				Destroy (this.gameObject);
			}
		}*/
	}

	void OnTriggerStay(Collider other)
	{
		if (!reachedEndOfLeash) 
		{
			if (!isDead) 
			{
				checkForPlayer (other);
			}	
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player) 
		{
			playerDetected = false;
			rotInterp = 0.0f;
			updateAnimation ("Speed", 0.0f);
		}
	}


	public void adjHealth(float adj)
	{
		health += adj;

		if (health <= 0.0f) 
		{
			isDead = true;
            health = 0.0f;
			Die ();
		}
	}
		

	public float getHealthRatio()
	{
		return health / maxHealth;
	}

	public float getDamage()
	{
		return damage;
	}

	public void Die()
	{
		updateAnimation ("IsDead");
		enemyCapCol.enabled = false;
        moveSpeed = 0;
	}

	public void checkForPlayer(Collider other)
	{
		if (other.gameObject == player) 
		{
			direction = other.transform.position - transform.position;

			float angleOfPlayer = Vector3.Angle(transform.forward, direction);

			if(angleOfPlayer <= (fieldOfView / 2))
			{
				if(!playerDetected)
					playerDetected = true;
			}
		}
	}

	public void moveToLocation()
	{
		updateAnimation ("Speed", 1.0f);

		Quaternion enemyLookRotation = Quaternion.LookRotation (transform.forward);
		Quaternion lookAtRot = Quaternion.LookRotation (new Vector3 (direction.x, 0.0f, direction.z));

		transform.rotation = Quaternion.Slerp (enemyLookRotation, lookAtRot, rotInterp);

		rotInterp += Time.deltaTime / 2;

		//Vector3 moveDirection = new Vector3 (direction.x, -gravity, direction.z);

		aiNavAgent.Move (direction.normalized * moveSpeed * Time.deltaTime);

		if (rotInterp >= 1.0f)
			rotInterp = 0.0f;

	}
	public void returnToOrigin()
	{
		direction = origin - transform.position;

		if (Vector3.Distance (player.transform.position, transform.position) > playerDistToSelf) 
		{
			updateAnimation("IsBackingUp", false);
			moveToLocation ();
		}

		else if((Vector3.Distance(origin, player.transform.position) < maxDistanceFromOriginAllowed)
			&& (Vector3.Distance(player.transform.position, transform.position) < playerDistToSelf))
		{
			direction = player.transform.position - transform.position;
			updateAnimation("IsBackingUp", false);
			updateAnimation ("Speed", 1.0f);
			moveToLocation ();
		}

		else 
		{
			//backAway ();
			aiNavAgent.Move (direction.normalized * (moveSpeed / 2) * Time.deltaTime);
			updateAnimation ("IsBackingUp", true);
			transform.LookAt (player.transform.position);
			//aiNavAgent.destination = origin;
		}
			
		if (curDistanceFromOrigin < 1.0f) 
		{
			reachedEndOfLeash = false;
			updateAnimation ("Speed", 0.0f);
		}
			
	}

	public void updateAnimation(string animFloatName, float value)
	{
		anim.SetFloat (animFloatName, value);
	}

	public void updateAnimation(string animBoolName, bool value)
	{
		anim.SetBool (animBoolName, value);
	}

	public void updateAnimation(string animTriggerName)
	{
		anim.SetTrigger (animTriggerName);
	}
}

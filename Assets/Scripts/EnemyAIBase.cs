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
		enemySphere = GetComponent<SphereCollider> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator> ();
		aiNavAgent = GetComponent<NavMeshAgent> ();
		aiNavAgent.speed = moveSpeed;
		isDead = false;
		enemyCapCol = GetComponent<CapsuleCollider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isDead) {
			adjHealth (-Time.deltaTime);
		} 
		else 
		{
			despawnTimer -= Time.deltaTime;

			if (despawnTimer <= 0.0f) 
			{
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (!isDead) 
		{
			if (other.gameObject == player) 
			{
				direction = other.transform.position - transform.position;

				float angleOfPlayer = Vector3.Angle(transform.forward, direction);

				//print (angleOfPlayer);

				if(angleOfPlayer <= (fieldOfView / 2))
				{
					if(!playerDetected)
						playerDetected = true;
					anim.SetFloat ("Speed", 1.0f);

					Quaternion enemyLookRotation = Quaternion.LookRotation (transform.forward);
					Quaternion lookAtPlayerRot = Quaternion.LookRotation (new Vector3 (direction.x, 0.0f, direction.z));

					transform.rotation = Quaternion.Slerp (enemyLookRotation, lookAtPlayerRot, rotInterp);

					rotInterp += Time.deltaTime / 5;

					//Vector3 moveDirection = new Vector3 (direction.x, -gravity, direction.z);

					aiNavAgent.Move (direction * moveSpeed * Time.deltaTime);


				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player) 
		{
			playerDetected = false;
			rotInterp = 0.0f;
			anim.SetFloat ("Speed", 0.0f);
		}
	}


	public void adjHealth(float adj)
	{
		health += adj;

		if (health <= 0.0f) 
		{
			isDead = true;
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

	void Die()
	{
		anim.SetTrigger("IsDead");
		enemyCapCol.enabled = false;
	}
}

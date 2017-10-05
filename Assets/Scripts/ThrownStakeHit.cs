using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownStakeHit : MonoBehaviour
{
    public Rigidbody rb;
    private EnemyAIBase enemyStats;
    public float damage;
    public float lifeTimer;

    
    public void setTrajectory(Vector3 direction, float throwForce)
    {
        this.rb.AddForce((direction * throwForce));
        
    }

    private void Update()
    {
        if (lifeTimer <= 0.0f)
            Destroy(this.gameObject);
        else
            lifeTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if((collision.collider.tag == "Enemy") && (collision.collider.GetType() == typeof(CapsuleCollider)))
        {
            print("Enemy Hit");
            enemyStats = collision.collider.GetComponent<EnemyAIBase>();
            enemyStats.adjHealth(-damage);
        }

        Destroy(this.gameObject);
    }
}

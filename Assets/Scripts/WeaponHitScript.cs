using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScript : MonoBehaviour
{
    public float damage;
    public float appliedForce;
    private bool canDamage;
    private EnemyAIBase enemyStats;
    private List<GameObject> hitEnemies;

    void Start()
    {
        hitEnemies = new List<GameObject>();
        canDamage = true;
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward, Color.red, 10.0f);

        if (canDamage)
            checkForEnemyHit();
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (canDamage)
        {
            if ((other.gameObject.tag == "Enemy") && !(hitEnemies.Contains(other.gameObject)))
            {
                print("Enemy Hit");
                enemyStats = other.GetComponent<EnemyAIBase>();
                enemyStats.adjHealth(-damage);
                hitEnemies.Add(other.gameObject);
            }
        }
    }*/

    public void turnDamageOff()
    {
        canDamage = false;
        hitEnemies.Clear();
    }
    
    public void turnDamageOn()
    {
        canDamage = true;
    }

    public void checkForEnemyHit()
    {
        RaycastHit Hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red, Time.deltaTime);
        if (Physics.Raycast(transform.position, transform.forward, out Hit, 1.5f))
        {
            if ((Hit.collider.tag == "Enemy") && !(hitEnemies.Contains(Hit.collider.gameObject)))
            {
                print("Enemy Hit");
                enemyStats = Hit.collider.GetComponent<EnemyAIBase>();
                enemyStats.adjHealth(-damage);
                hitEnemies.Add(Hit.collider.gameObject);
            }

        }
    }
}

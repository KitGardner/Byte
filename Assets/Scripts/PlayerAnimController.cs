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
		anim = GetComponentInChildren<Animator> ();
	}

    #region animation switching functions
    public void setSpeedValue(float value)
	{
		speed = value;
        anim.SetFloat("Speed", speed);
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

    public void greatSwordComboAttack(bool isAttacking, bool usingGreatsword, int lightAttackCount)
    {
        anim.SetBool("Attacking", isAttacking);
        anim.SetBool("Using Greatsword", usingGreatsword);
        anim.SetInteger("Light Attack Count", lightAttackCount);      
    }

    public void isAttacking(bool isAttacking, string weaponName)
    {
        anim.SetBool("Attacking", isAttacking);

        switch (weaponName)
        {
            case "Sword":
                anim.SetBool("Using Greatsword", isAttacking);
                break;
            case "Hammer":
                anim.SetBool("Using Warhammer", isAttacking);
                break;
            case "Double Sickle":
                anim.SetBool("Using Double Sickle", isAttacking);
                break;
            default:
                break;
        }
    }

    public void usingLeftArm(bool usingArm)
    {
        anim.SetBool("Use Left Arm", usingArm);
    }

    public void lightAttack()
    {
        anim.SetTrigger("Light Attack");
    }

    public void heavyAttack()
    {
        anim.SetTrigger("Heavy Attack");
    }

    public void throwStake()
    {
        anim.SetTrigger("Throwing Stakes");
    }

    public void chargeArm(bool charging)
    {
        anim.SetBool("Arm Charging", charging);
    }

    public void dischargeArm()
    {
        anim.SetTrigger("Discharge Arm");
    }

    public void continuousDischarge(bool discharging)
    {
        anim.SetBool("Arm Discharging", discharging);
    }

    public void grabEnemy()
    {
        anim.SetTrigger("Grab Enemy");
    }

    public void drainEnemy(bool draining)
    {
        anim.SetBool("Draining Enemy", draining);
    }
    #endregion




}

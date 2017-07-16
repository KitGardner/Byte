using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour 
{
	public PlayerStats playerStats;
	public Image healthBar;
	public Image armAbilityBar;
	public Image armAbilityIcon;
	public Sprite[] armAbilitySprites;

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () 
	{
		playerStats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		healthBar.fillAmount = playerStats.getHealthRatio();
		armAbilityBar.fillAmount = playerStats.getLeftArmChargeRatio ();
	}

	public void setAbilityIcon(int abilityIconNumber)
	{
		armAbilityIcon.sprite = armAbilitySprites [abilityIconNumber];
	}
}

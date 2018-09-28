using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsModel
{

    //health stats
    public float MaxHealth { get; set; }
    public float Health { get; set; }

    //virus stats
    public float MaxVirusAmt { get; set; }
    public float VirusAmt { get; set; }

    //Left Arm Stats
    public float MaxCharge { get; set; }
    public float CurCharge { get; set; }

    //Weapons Flags
    public bool HasGreatsword { get; set; }
    public bool HasGreatHammer { get; set; }
    public bool HasTwinSickle { get; set; }

    //Skill Flags
    public bool CanDrainBlood { get; set; }
    public bool CanThrowStakes { get; set; }
    public bool CanHarnessEnergy { get; set; }
    public bool CanSmashObject { get; set; }

    //Active equipment
    public string EquippedWeaponName { get; set; }
    public string EquippedSkillName { get; set; }

    public int SaveGameId { get; set; }
}

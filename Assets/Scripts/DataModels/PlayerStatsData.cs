using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStatsData : MonoBehaviour {
 
    public Guid SaveFileId { get; set; }
    public int MaxHealth { get; set; }
    public int CurHealth { get; set; }
    public int MaxEnergy { get; set; }
    public int CurEnergy { get; set; }
    public int MaxVirus { get; set; }
    public int CurVirus { get; set; }
    public Guid EquippedWeapon { get; set; }
    public Guid EquippedSkill { get; set; }
}

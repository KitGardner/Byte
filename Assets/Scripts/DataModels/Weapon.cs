using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Guid WeaponId { get; set; }
    public string WeaponName { get; set; }
    public string WeaponDescription { get; set; }
    public string WeaponInformation { get; set; }
    public int WeaponDamage { get; set; }
    public bool HasWeapon { get; set; }
}

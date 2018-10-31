using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Upgrade : MonoBehaviour {

    public Guid UpgradeId { get; set; }
    public string UpgradeName { get; set; }
    public string UpgradeDescription { get; set; }
    public bool HasUpgrade { get; set; }
}

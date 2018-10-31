using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SavePointData : MonoBehaviour {

    public Guid SavePointId { get; set; }
    public string SavePointName { get; set; }
    public Vector3 SpawnPos { get; set; }
    public Vector3 SpawnRot { get; set; }
}

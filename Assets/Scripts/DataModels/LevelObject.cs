using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelObject : MonoBehaviour {

    public Guid LevelId { get; set; }
    public string SceneName { get; set; }
    public string FilePath { get; set; }
}

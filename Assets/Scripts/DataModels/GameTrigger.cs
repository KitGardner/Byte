using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameTrigger : MonoBehaviour {

	public Guid TriggerId { get; set; }
    public string TriggerName { get; set; }
    public string TriggerDescription { get; set; }
    public bool Activated { get; set; }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill : MonoBehaviour {
    
    public Guid SkillId { get; set; }
    public string SkillName { get; set; }
    public string SkillDescription { get; set; }
    public bool HasSkill { get; set; }
}

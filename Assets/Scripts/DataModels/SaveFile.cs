using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SaveFile : MonoBehaviour {

    public Guid SaveFileId { get; set; }
    public DateTime CreatedUtcDate { get; set; }
    public DateTime CreatedLocalTime { get; set; }
    public string Difficulty { get; set; }
    public string ScreenShotFilePath { get; set; }
    public TimeSpan PlayTime { get; set; }
    public Guid LevelId { get; set; }
    public Guid SavePointId { get; set; }
}

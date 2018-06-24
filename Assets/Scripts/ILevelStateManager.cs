using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelStateManager
{
    string LevelName { get; }

    bool InitializeLevel();
}

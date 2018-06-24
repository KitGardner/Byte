using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuvallEstateExtManager : MonoBehaviour, ILevelStateManager
{
    private const string levelName = "Duvall_Estate_Exterior_Scene";
    public string LevelName{ get { return levelName; } }

    public bool InitializeLevel()
    {
        throw new System.NotImplementedException();
    }

    public static bool ManorDoorUnlocked = false;


	

}

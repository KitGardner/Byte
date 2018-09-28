using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public int SaveGameId { get; set; }

    public string CurrentGameLevelName { get; set; }

    public int CurrentGamelevelId { get; set; }

    public float PlayTime { get; set; }

    public DateTime SaveDateTime { get; set; }

    public GameDifficulty GameDifficulty { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public enum GameDifficulty
{
    Easy = 10,
    Normal = 20,
    Hard = 30
}

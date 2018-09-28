using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveObject : MonoBehaviour
{

    public PlayerStatsModel PlayerStats { get; set; }

    public GameState GameState { get; set; }

    public Inventory Inventory { get; set; }

    public ILevelStateManager LevelStateManager { get; set; }

    public int SaveGameId { get; set; }

    public GameSaveObject(PlayerStatsModel playerStatsModel, GameState gameState, Inventory inventory, ILevelStateManager levelStateManager)
    {
        this.PlayerStats = playerStatsModel;
        this.GameState = gameState;
        this.Inventory = inventory;
        this.LevelStateManager = levelStateManager;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

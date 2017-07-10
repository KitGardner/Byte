using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void LoadScene(string levelName)
	{
		SceneManager.LoadScene (levelName);
	}

	public void Quit()
	{
		Application.Quit ();
	}
}

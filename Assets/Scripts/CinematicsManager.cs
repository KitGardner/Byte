using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsManager : MonoBehaviour
{
    public Camera PlayerCamera;
    public Camera CinematicCamera;
    public Cinematic cinematic;

	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public async void EnableCinematic()
    {
        print("I am enabling the Cinematic");
        PlayerCamera.enabled = false;
        CinematicCamera.enabled = true;
        InputManager.InputsEnabled = false;
        StartCoroutine("WaitForInput");

    }

    public IEnumerator WaitForInput()
    {
        yield return new WaitForSecondsRealtime(2);
        cinematic.PlayTest();
        yield return new WaitForSecondsRealtime(2);
        DisableCinematic();
    }

    public void DisableCinematic()
    {
        PlayerCamera.enabled = true;
        CinematicCamera.enabled = false;
        InputManager.InputsEnabled = true;
    }
}

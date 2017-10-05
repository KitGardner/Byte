using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public InputManager inputManager;

    public void setPlayerInteractivity(bool interacting)
    {
        inputManager.setPlayerInteraction(interacting, this);
    }

    public abstract void Interact();
}

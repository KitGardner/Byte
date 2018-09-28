using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public InputManager inputManager;

    public void setPlayerInteractivity(bool interacting, CharacterMovement charMove, InteractiveObject intObj)
    {
        charMove.setPlayerInteraction(interacting, intObj);
    }

    public abstract void Interact();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTrigger : MonoBehaviour
{
    public int interactionID; // value set in Unity Editor
    public InteractionType interactionType; // value set in Unity Editor

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && EventManager.Mode == GameMode.PLAY)
        {
            EventManager.ProcessInteraction(interactionID, interactionType);
        }
    }
}

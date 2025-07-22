using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTrigger : MonoBehaviour
{
    public int interactionID; // value set in Unity Editor
    public InteractionType interactionType; // value set in Unity Editor

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            RoomManager.Instance.ProcessInteraction(interactionID, interactionType);
        }
    }
}

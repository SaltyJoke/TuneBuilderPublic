using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTrigger : MonoBehaviour
{
    RoomManager manager;
    public InteractionType interactionType; // value set in Unity Editor

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize()
    {
        manager = (RoomManager)(GameObject.Find("RoomManager").gameObject.GetComponent<MonoBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            manager.ProcessInteraction(name, interactionType);
        }
    }
}

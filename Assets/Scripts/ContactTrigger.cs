using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTrigger : MonoBehaviour
{
    RoomManager manager;
    int roomIndex;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize()
    {
        manager = (RoomManager)(GameObject.Find("RoomManager").gameObject.GetComponent<MonoBehaviour>());
        Room parent = (Room)(transform.parent.GetComponent<MonoBehaviour>());
        roomIndex = parent.roomIndex;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            manager.ProcessInteraction(name, roomIndex, transform.GetSiblingIndex());
        }
    }
}

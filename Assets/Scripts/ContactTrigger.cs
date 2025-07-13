using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactTrigger : MonoBehaviour
{
    RoomManager manager;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize()
    {
        manager = (RoomManager)(GameObject.Find("RoomManager").gameObject.GetComponent<MonoBehaviour>());
        UnityEngine.Debug.Log("Initialize " + transform.GetSiblingIndex());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            manager.ProcessInteraction(name, transform.parent.name, transform.GetSiblingIndex());
        }
    }
}

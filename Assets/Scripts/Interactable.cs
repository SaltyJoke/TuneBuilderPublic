using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool active = false;
    RoomManager manager;
    Sprite activeSprite;
    int roomIndex;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activeSprite = Resources.Load<Sprite>("Visuals/" + gameObject.name + "-interact");
        Deactivate();
        manager = (RoomManager)(GameObject.Find("RoomManager").gameObject.GetComponent<MonoBehaviour>());
        Room parent = (Room)(transform.parent.GetComponent<MonoBehaviour>());
        roomIndex = parent.roomIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyUp(KeyCode.C))
        {
            manager.ProcessInteraction(name, roomIndex, transform.GetSiblingIndex());
        }
    }

    public void Activate()
    {
        spriteRenderer.sprite = activeSprite;
        UnityEngine.Debug.Assert(spriteRenderer.sprite);
        active = true;
    }

    public void Deactivate()
    {
        spriteRenderer.sprite = null;
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Activate();
            UnityEngine.Debug.Log("Enter Interactable");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Deactivate();
        }
    }
}

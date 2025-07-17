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
    public InteractionType interactionType; // value set in Unity Editor

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
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyUp(KeyCode.C))
        {
            manager.ProcessInteraction(name, interactionType);
        }
    }

    public void Activate()
    {
        spriteRenderer.sprite = activeSprite;
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

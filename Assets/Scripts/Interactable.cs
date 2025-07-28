using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool active = false;
    Sprite activeSprite;
    public int interactionID; // value set in Unity Editor
    public InteractionType interactionType; // value set in Unity Editor

    public void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activeSprite = Resources.Load<Sprite>("Visuals/" + gameObject.name + "-interact");
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyUp(KeyCode.C) && EventManager.Mode == GameMode.PLAY)
        {
            EventManager.ProcessInteraction(interactionID, interactionType);
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

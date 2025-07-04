using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool active = false;
    bool itemCollected = false;
    TuneCollection tuneMenu;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Deactivate();
        tuneMenu = (TuneCollection)(GameObject.Find("TuneMenu").gameObject.GetComponent<MonoBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !itemCollected && Input.GetKeyUp(KeyCode.C))
        {
            tuneMenu.AddFragment(gameObject.name);
            itemCollected = true;
        }
    }

    public void Activate()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>("Visuals/" + gameObject.name + "-interact");
        active = true;
    }

    public void Deactivate()
    {
        spriteRenderer.sprite = null;
        active = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Deactivate();
        }
    }
}

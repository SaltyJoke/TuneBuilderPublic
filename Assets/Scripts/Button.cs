using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ButtonAction action; // value set in Unity Editor
    private SpriteRenderer spriteRenderer;
    private Sprite activeSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        activeSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = null;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.sprite = activeSprite;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.ProcessButtonPress(action);
        }
    }

    private void OnMouseExit()
    {
        spriteRenderer.sprite = null;
    }
}

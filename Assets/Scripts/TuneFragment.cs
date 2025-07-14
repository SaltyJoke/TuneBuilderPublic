using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneFragment : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;
    Sprite interactiveSprite;
    bool interacting;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        interactiveSprite = Resources.Load<Sprite>("Visuals/" + spriteRenderer.sprite.name + "-interact");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ToggleInteraction();
        }
    }

    private void ToggleInteraction()
    {
        if (!interacting)
        {
            spriteRenderer.sprite = interactiveSprite;
            interacting = true;
        } else
        {
            spriteRenderer.sprite = defaultSprite;
            interacting = false;
        }
    }
}

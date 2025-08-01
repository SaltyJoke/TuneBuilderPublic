using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneFragment : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite interactiveSprite;
    [SerializeField] public AudioClip audioClip;
    public FragmentType fragmentType;
    bool interacting;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SendMessageUpwards("ToggleInteraction", transform.GetSiblingIndex());
        }
    }

    public void ToggleInteraction()
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

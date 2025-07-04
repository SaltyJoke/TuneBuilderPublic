using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private enum PlayerAnimationState
    {
        STAND, START, WALK, STOP
    }
    private enum ScrollState
    {
        WALK, SCROLL, STOP
    }
    private PlayerAnimationState animationState = PlayerAnimationState.STAND;
    private ScrollState scrollState = ScrollState.WALK;
    private string walkPrefix;
    private int walkSpriteIndex;
    private float velocity;
    private const float ZERO = 0.0001f;
    private int direction;
    Room room;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Visuals/player-placeholder");
        spriteRenderer.flipX = false;
        walkPrefix = "Visuals/player-placeholder-walk";
        walkSpriteIndex = 0;
        velocity = 0f;
        direction = 1;
        room = (Room)(GameObject.FindWithTag("Room").GetComponent<MonoBehaviour>());
        Time.fixedDeltaTime = 0.1f; // TODO: move this to a better spot 
    }

    private void HandleInput()
    {
        float walkInput = Input.GetAxis("Horizontal");
        if (walkInput > ZERO)
        {
            direction = 1;
        } else if (walkInput < ZERO * -1f)
        {
            direction = -1;
        }
        if (Math.Abs(walkInput) > ZERO)
        {
            if (animationState == PlayerAnimationState.STAND || animationState == PlayerAnimationState.STOP)
            {
                animationState = PlayerAnimationState.START;
            }
        }
        else
        {
            if (animationState == PlayerAnimationState.WALK || animationState == PlayerAnimationState.START)
            {
                animationState = PlayerAnimationState.STOP;
            }
        }
    }

    private void UpdateVelocity()
    {
        if (animationState == PlayerAnimationState.WALK)
        {
            velocity = 4f * direction;
        } else if (animationState == PlayerAnimationState.STAND)
        {
            velocity = 0f;
        } else
        {
            velocity = 0.5f * direction;
        }
    }

    private void UpdatePosition()
    {
        // TODO: add scrolling functionality
        if (scrollState == ScrollState.WALK)
        {
            if ((velocity > 0f && transform.position.x > 0f && transform.position.x < 0.5f) 
                || (velocity < 0f && transform.position.x < 0f && transform.position.x > -0.5f)) // hit middle of screen
            {
                scrollState = ScrollState.SCROLL;
            } else
            {
                transform.Translate(Vector3.right * velocity * Time.deltaTime);
            }
        }
        if (scrollState == ScrollState.SCROLL)
        {
            bool scrollable = room.Scroll(velocity);
            if (!scrollable)
            {
                scrollState = ScrollState.WALK;
            }
        }
        if (scrollState == ScrollState.STOP
            && ((velocity > 0f && transform.position.x < 0f) || (velocity < 0f && transform.position.x > 0f)))
        {
            transform.Translate(Vector3.right * velocity * Time.deltaTime);
            scrollState = ScrollState.WALK;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateVelocity();
        UpdatePosition();
    }

    private void updateAnimation()
    {
        spriteRenderer.flipX = direction == -1;
        // TODO: update sprite before updating animation state to ensure at least one animation frame before sprite change 
        switch (animationState)
        {
            case PlayerAnimationState.STAND:
                spriteRenderer.sprite = Resources.Load<Sprite>("Visuals/player-placeholder");
                break;
            case PlayerAnimationState.START:
                spriteRenderer.sprite = Resources.Load<Sprite>("Visuals/player-placeholder-transition");
                break;
            case PlayerAnimationState.WALK:
                spriteRenderer.sprite = Resources.Load<Sprite>(walkPrefix + walkSpriteIndex);
                walkSpriteIndex = (walkSpriteIndex + 1) % 4;
                break;
            case PlayerAnimationState.STOP:
                spriteRenderer.sprite = Resources.Load<Sprite>("Visuals/player-placeholder-transition");
                break;
        }
        if (animationState == PlayerAnimationState.START)
        {
            animationState = PlayerAnimationState.WALK;
            walkSpriteIndex = 0;
        }
        if (animationState == PlayerAnimationState.STOP)
        {
            animationState = PlayerAnimationState.STAND;
        }
    }

    void FixedUpdate()
    {
        updateAnimation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "EndOfRoom")
        {
            scrollState = ScrollState.STOP;
            UnityEngine.Debug.Log("End of room reached");
        }
    }
}

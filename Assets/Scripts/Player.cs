using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private enum PlayerAnimationState
    {
        STAND, START, WALK, STOP
    }
    private PlayerAnimationState animationState = PlayerAnimationState.STAND;
    private int walkSpriteIndex;
    [SerializeField] Sprite[] sprites;

    private float velocity;
    private const float ZERO = 0.0001f;
    private int direction;
    Room room;
    Vector2 endOfRoom;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkSpriteIndex = 0;
        spriteRenderer.sprite = sprites[0];
        spriteRenderer.flipX = false;

        velocity = 0f;
        direction = 1;
        Time.fixedDeltaTime = 0.1f; // TODO: move this to a better spot 
    }

    public void EnterRoom(Room r, int entryIndex)
    {
        room = r;
        endOfRoom = room.GetEndOfRoom();
        transform.Translate(Vector3.right * (r.roomInfo.playerStartingX[entryIndex] - transform.position.x));
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
        if ((velocity == 0f) || 
            (velocity > 0f && transform.position.x >= endOfRoom.y) || (velocity < 0f && transform.position.x <= endOfRoom.x))
        {
            return;
        }
        if ((velocity > 0f && transform.position.x < 0f) || (velocity < 0f && transform.position.x > 0f))
        {
            transform.Translate(Vector3.right * velocity * Time.deltaTime);
        } else
        {
            bool scrollable = room.Scroll(velocity);
            if (!scrollable)
            {
                transform.Translate(Vector3.right * velocity * Time.deltaTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventManager.Mode == GameMode.PLAY)
        {
            HandleInput();
            UpdateVelocity();
            UpdatePosition();
        }
    }

    private void UpdateAnimation()
    {
        spriteRenderer.flipX = direction == -1;
        switch (animationState)
        {
            case PlayerAnimationState.STAND:
                spriteRenderer.sprite = sprites[0];
                break;
            case PlayerAnimationState.START:
                spriteRenderer.sprite = sprites[1];
                break;
            case PlayerAnimationState.WALK:
                spriteRenderer.sprite = sprites[2 + walkSpriteIndex];
                walkSpriteIndex = (walkSpriteIndex + 1) % 4;
                break;
            case PlayerAnimationState.STOP:
                spriteRenderer.sprite = sprites[1];
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
        if (EventManager.Mode == GameMode.PLAY)
        {
            UpdateAnimation();
        }
    }
}

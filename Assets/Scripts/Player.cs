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
    private string walkPrefix;
    private int walkSpriteIndex;
    private float velocity;
    private const float ZERO = 0.0001f;
    private int direction;
    Room room;
    Vector2 endOfRoom;
    //double deltaTimeSum = 0;
    //double deltaTimeSum2 = 0;

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
        Time.fixedDeltaTime = 0.1f; // TODO: move this to a better spot 
        //StartCoroutine(changeFramerate()); // TODO: move this to a better spot 
    }

    /*
    IEnumerator changeFramerate()
    {
        yield return new WaitForSeconds(1);
        QualitySettings.vSyncCount = 0;
        UnityEngine.Application.targetFrameRate = 60;
    }
    */

    public void EnterRoom(Room r, float x)
    {
        room = r;
        endOfRoom = room.getEndOfRoom();
        UnityEngine.Debug.Log(transform.position.x + " to " + x);
        transform.Translate(Vector3.right * (x - transform.position.x));
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
}

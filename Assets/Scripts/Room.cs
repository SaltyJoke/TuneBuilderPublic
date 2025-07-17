using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomIndex;
    public RoomInfo roomInfo; // value set in Unity Editor
    float minX;
    float maxX;
    Vector2 endOfRoom;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitializeRoom(int entryIndex)
    {
        transform.Translate(Vector3.right * (roomInfo.roomStartingX[entryIndex] - transform.position.x));
        spriteRenderer = GetComponent<SpriteRenderer>();
        Camera camera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>();
        float cameraHalfWidth = camera.orthographicSize * camera.aspect;
        maxX = cameraHalfWidth * -1;
        minX = -1 * spriteRenderer.sprite.bounds.size.x + cameraHalfWidth;
        endOfRoom = new Vector2(maxX + roomInfo.leftBarrierOffset, cameraHalfWidth - roomInfo.rightBarrierOffset); // player movement bounds; unique to room
        transform.BroadcastMessage("Initialize");
    }

    public Vector2 GetEndOfRoom()
    {
        return endOfRoom;
    }

    // Returns whether the background can scroll further in this direction or not
    public bool Scroll(float velocity)
    {
        if (transform.position.x - velocity * Time.deltaTime < minX)
        {
            transform.Translate(Vector3.right * (minX - transform.position.x));
            return false;
        }
        if (transform.position.x - velocity * Time.deltaTime > maxX)
        {
            transform.Translate(Vector3.right * (maxX - transform.position.x));
            return false;
        }
        transform.Translate(Vector3.right * velocity * Time.deltaTime * -1);
        return true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

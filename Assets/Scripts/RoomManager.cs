using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomInfo
{
    public GameObject room;
    public float[] playerStartingX;
    public float[] roomStartingX;
    public float leftBarrierOffset; // leftmost player offset from left of room
    public float rightBarrierOffset; // rightmost player offset from right of room
    public int[] interactionType; // what happens when each child is interacted with
}

public class RoomManager : MonoBehaviour
{
    Player player;
    TuneCollection tuneCollection;
    Room currentRoom;
    RoomInfo[] roomInfo = new RoomInfo[4];
    readonly Dictionary<string, int[]> roomConnections = new Dictionary<string, int[]>()
    {
        { "Start", new int[] { 0, 1 } },
        { "KitchenExitLeft", new int[] { 1, 2 } },
        { "HallwayExitRight", new int[] { 0, 0 } },
        { "door", new int[] { 2, 0 } },
        { "BedroomExitLeft", new int[] { 1, 1 } }
    };
    // Start is called before the first frame update
    void Start()
    {
        player = (Player)(GameObject.Find("Player").GetComponent<MonoBehaviour>());
        tuneCollection = (TuneCollection)(GameObject.Find("TuneMenu").gameObject.GetComponent<MonoBehaviour>());

        roomInfo[0] = new RoomInfo()
        {
            room = GameObject.Find("Kitchen"),
            playerStartingX = new float[] { -9f, -7.5f },
            roomStartingX = new float[] { -9.6f, -9.6f },
            leftBarrierOffset = -0.5f,
            rightBarrierOffset = 3f,
            interactionType = new int[] { 0, 1, 1, 1 }
        };
        roomInfo[1] = new RoomInfo()
        {
            room = GameObject.Find("Hallway"),
            playerStartingX = new float[] { -9f, -3f, 9f },
            roomStartingX = new float[] { -9.6f, -9.6f, -23.41f }, // TODO: un-magic the right of the room
            leftBarrierOffset = -0.5f,
            rightBarrierOffset = -0.5f,
            interactionType = new int[] { -1, 0, 0, 0, 2 }
        };
        roomInfo[2] = new RoomInfo()
        {
            room = GameObject.Find("Bedroom"),
            playerStartingX = new float[] { -9f },
            roomStartingX = new float[] { -9.6f },
            leftBarrierOffset = -0.5f,
            rightBarrierOffset = 1f,
            interactionType = new int[] { -1, 0, 1, 1 }
        };

        TransitionRoom("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessInteraction(string interactionName, int room, int index)
    {
        if (roomInfo[room].interactionType[index] == 0)
        {
            TransitionRoom(interactionName);
        } else
        {
            tuneCollection.AddFragment(interactionName);
        }
    }

    void TransitionRoom(string transitionName)
    {
        int index = roomConnections[transitionName][0];
        int enterIndex = roomConnections[transitionName][1];
        for (int i = 0; i < roomInfo.Length; i++)
        {
            GameObject r = roomInfo[i].room;
            if (i == index)
            {
                r.SetActive(true);
                currentRoom = (Room)(r.GetComponent<MonoBehaviour>());
                currentRoom.InitializeInfo(index, roomInfo[index].roomStartingX[enterIndex], roomInfo[index].leftBarrierOffset, roomInfo[index].rightBarrierOffset);
                player.EnterRoom(currentRoom, roomInfo[index].playerStartingX[enterIndex]);
            } else
            {
                r.SetActive(false); // TODO: look into whether unloading rooms completely is wanted
            }
        }
        
    }
}

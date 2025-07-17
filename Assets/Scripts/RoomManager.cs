using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoomInfo
{
    public int index;
    public float[] playerStartingX;
    public float[] roomStartingX;
    public float leftBarrierOffset; // leftmost player offset from left of room
    public float rightBarrierOffset; // rightmost player offset from right of room
}

public enum InteractionType
{
    ROOM_TRANSITION, ADD_FRAGMENT, CD_PLAYER, EXIT_GAME
};

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
        { "BedroomExitLeft", new int[] { 1, 1 } },
        { "HallwayExitLeft", new int[] { 3, 0 } },
        { "BathroomExitRight", new int[] { 1, 0 } }
    };
    GameObject[] rooms = new GameObject[4];

    void Awake()
    {
        rooms[0] = GameObject.Find("Kitchen");
        rooms[1] = GameObject.Find("Hallway");
        rooms[2] = GameObject.Find("Bedroom");
        rooms[3] = GameObject.Find("Bathroom");
        foreach (GameObject room in rooms)
        {
            room.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = (Player)(GameObject.Find("Player").GetComponent<MonoBehaviour>());
        tuneCollection = (TuneCollection)(GameObject.Find("TuneMenu").gameObject.GetComponent<MonoBehaviour>());

        TransitionRoom("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessInteraction(string interactionName, InteractionType type)
    {
        switch (type)
        {
            case InteractionType.ROOM_TRANSITION:
                TransitionRoom(interactionName);
                break;
            case InteractionType.ADD_FRAGMENT:
                tuneCollection.AddFragment(interactionName);
                break;
            case InteractionType.CD_PLAYER:
                UnityEngine.Debug.Log("CD player not implemented");
                break;
            case InteractionType.EXIT_GAME:
                UnityEngine.Debug.Log("Exit game not implemented");
                break;
        }
    }

    void TransitionRoom(string transitionName)
    {
        int index = roomConnections[transitionName][0];
        int entryIndex = roomConnections[transitionName][1];
        for (int i = 0; i < rooms.Length; i++)
        {
            if (i == index)
            {
                rooms[i].SetActive(true);
                currentRoom = (Room)(rooms[i].GetComponent<MonoBehaviour>());
                currentRoom.InitializeRoom(entryIndex);
                player.EnterRoom(currentRoom, entryIndex);
            } else
            {
                rooms[i].SetActive(false); // TODO: look into whether unloading rooms completely is wanted
            }
        }
        
    }
}

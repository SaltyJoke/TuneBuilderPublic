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

    // room transition order: start, kitchen, hallway left / center / right, bedroom, bathroom
    [SerializeField] int[] destinationRooms = new int[] { 0, 1, 3, 2, 0, 1, 1 };
    [SerializeField] int[] destinationEntryIndices = new int[] { 1, 2, 0, 0, 0, 1, 0 };
    [SerializeField] GameObject[] rooms = new GameObject[4];

    public static RoomManager Instance;

    void Awake()
    {
        Instance = this;

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

        TransitionRoom(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessInteraction(int interactionID, InteractionType type)
    {
        switch (type)
        {
            case InteractionType.ROOM_TRANSITION:
                TransitionRoom(interactionID);
                break;
            case InteractionType.ADD_FRAGMENT:
                tuneCollection.AddFragment(interactionID);
                break;
            case InteractionType.CD_PLAYER:
                UnityEngine.Debug.Log("CD player not implemented");
                break;
            case InteractionType.EXIT_GAME:
                UnityEngine.Debug.Log("Exit game not implemented");
                break;
        }
    }

    void TransitionRoom(int transitionID)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (i == destinationRooms[transitionID])
            {
                rooms[i].SetActive(true);
                currentRoom = (Room)(rooms[i].GetComponent<MonoBehaviour>());
                currentRoom.InitializeRoom(destinationEntryIndices[transitionID]);
                player.EnterRoom(currentRoom, destinationEntryIndices[transitionID]);
            } else
            {
                rooms[i].SetActive(false); // TODO: look into whether unloading rooms completely is wanted
            }
        }
        
    }
}

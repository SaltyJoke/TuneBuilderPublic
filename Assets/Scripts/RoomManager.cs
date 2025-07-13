using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomInfo
{
    public float playerStartingX;
    public float leftBarrierOffset;
    public float rightBarrierOffset;
    public int[] interactionType; // what happens when each child is interacted with
}

public class RoomManager : MonoBehaviour
{
    Player player;
    TuneCollection tuneCollection;
    Room currentRoom;
    Dictionary<string, RoomInfo> roomInfo;
    Dictionary<string, string> roomConnections = new Dictionary<string, string>()
    {
        { "Start", "Kitchen" },
        { "KitchenExitLeft", "DiningRoom" }
    };
    // Start is called before the first frame update
    void Start()
    {
        player = (Player)(GameObject.Find("Player").GetComponent<MonoBehaviour>());
        tuneCollection = (TuneCollection)(GameObject.Find("TuneMenu").gameObject.GetComponent<MonoBehaviour>());
        roomInfo = new Dictionary<string, RoomInfo>();
        int[] it = { 0, 1, 1, 1 };
        roomInfo.Add("Kitchen", new RoomInfo()
        {
            playerStartingX = -7.5f, leftBarrierOffset = -0.5f, rightBarrierOffset = 3, interactionType = it
        });
        TransitionRoom("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessInteraction(string interactionName, string room, int index)
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
        string destination = roomConnections[transitionName];
        UnityEngine.Debug.Log(destination);
        currentRoom = (Room)(GameObject.Find(destination).GetComponent<MonoBehaviour>());
        currentRoom.InitializeInfo(roomInfo[destination]);
        player.EnterRoom(currentRoom, roomInfo[destination].playerStartingX);
    }
}

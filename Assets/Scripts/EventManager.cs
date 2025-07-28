using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    PAUSE, PLAY // inherent potential for there to be more detailed modes of operation
};

public enum InteractionType
{
    ROOM_TRANSITION, ADD_FRAGMENT, CD_PLAYER, END_GAME
};

public enum ButtonAction
{
    START_GAME, QUIT_APPLICATION, INTERACTION_SUBSTITUTE
};

public static class EventManager
{
    public static GameMode Mode = GameMode.PAUSE;

    public static void ProcessInteraction(int interactionID, InteractionType type)
    {
        switch (type)
        {
            case InteractionType.ROOM_TRANSITION:
                RoomManager.Instance.TransitionRoom(interactionID);
                break;
            case InteractionType.ADD_FRAGMENT:
                TuneCollection.Instance.AddFragment(interactionID);
                break;
            case InteractionType.CD_PLAYER:
                UnityEngine.Debug.Log("CD player not implemented");
                break;
            case InteractionType.END_GAME:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public static void ProcessButtonPress(ButtonAction action)
    {
        switch (action)
        {
            case ButtonAction.START_GAME:
                SceneManager.LoadScene("MainScene");
                break;
            case ButtonAction.QUIT_APPLICATION:
                Application.Quit();
                break;
            case ButtonAction.INTERACTION_SUBSTITUTE:
                UnityEngine.Debug.Log("Why are you here");
                break;
        }
    }
}

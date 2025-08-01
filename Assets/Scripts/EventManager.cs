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
    START_GAME, QUIT_APPLICATION, FRAGMENT_MENU, TUNE_MENU, INTERACTION_SUBSTITUTE
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
                TuneCollection.Instance.OpenTuneMenu(true);
                break;
            case InteractionType.END_GAME:
                SceneManager.LoadScene("MainMenu");
                Mode = GameMode.PAUSE;
                break;
        }
    }

    public static void ProcessButtonPress(ButtonAction action)
    {
        switch (action)
        {
            case ButtonAction.START_GAME:
                SceneManager.LoadScene("MainScene");
                Mode = GameMode.PLAY;
                break;
            case ButtonAction.QUIT_APPLICATION:
                Application.Quit();
                break;
            case ButtonAction.FRAGMENT_MENU:
                TuneCollection.Instance.OpenFragmentMenu();
                break;
            case ButtonAction.TUNE_MENU:
                TuneCollection.Instance.OpenTuneMenu();
                break;
            case ButtonAction.INTERACTION_SUBSTITUTE:
                UnityEngine.Debug.Log("Why are you here");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ButtonAction action; // value set in Unity Editor

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.ProcessButtonPress(action);
        }
    }
}

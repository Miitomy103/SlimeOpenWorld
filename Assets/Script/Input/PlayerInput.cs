using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct PlayerInput
{
    public Vector2 Move => new Vector2(Horizontal, Vertical);
    public float Horizontal;
    public float Vertical;
    public Vector2 Axis;
    public InputButton Jump;
    public InputButton Action0;
    public InputButton Action1;
    public InputButton Button0;
    public InputButton Button1;
    public InputButton Button2;
    /// <summary>
    /// TKey
    /// </summary>
    public InputButton Button3;
    public InputButton Interact;
}
public struct InputButton
{
    public bool onDown;
    public bool onButton;
    public bool onUp;

    public InputButton(string buttonName)
    {
        onDown = Input.GetButtonDown(buttonName);
        onButton = Input.GetButton(buttonName);
        onUp = Input.GetButtonUp(buttonName);
    }
    public InputButton(InputAction action)
    {
        onDown = action.WasPressedThisFrame();
        onButton = action.IsPressed();
        onUp = action.WasReleasedThisFrame();
    }
}

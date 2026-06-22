using UnityEngine;

/// <summary>
/// InputSystemPlayerをラップし、現在フレームのPlayerInputをまとめて取得できるシングルトン。
/// UI操作中(IsUsingUI)は入力を無効化する。
/// </summary>
public class InputData : MonoBehaviour
{
    public InputSystemPlayer inputActions;

    private static InputData instance;
    public static InputData Instance => instance;

    public bool IsUsingUI { get; set; } = false;
    private void Awake()
    {
        instance = this;

        inputActions = new InputSystemPlayer();
        inputActions.Enable();
    }

    public PlayerInput InputAction()
    {
        PlayerInput input = new PlayerInput();

        if(IsUsingUI)
        {
            return input;
        }

        input.Horizontal =inputActions.Player.Move.ReadValue<Vector2>().x;
        input.Vertical = inputActions.Player.Move.ReadValue<Vector2>().y;

        input.Axis = inputActions.Player.Axis.ReadValue<Vector2>();
        input.Jump = new InputButton(inputActions.Player.Jump);

        input.Action0 = new InputButton(inputActions.Player.Action0);
        input.Action1 = new InputButton(inputActions.Player.Action1);

        input.Button0 = new InputButton(inputActions.Player.Button0);
        input.Button1 = new InputButton(inputActions.Player.Button1);
        input.Button2 = new InputButton(inputActions.Player.Button2);
        input.Button3 = new InputButton(inputActions.Player.Button3);
        input.Interact = new InputButton(inputActions.Player.Interact);
        return input;
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }
}


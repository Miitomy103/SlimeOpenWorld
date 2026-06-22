using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystemPlayerのMoveアクションを購読し、移動入力をMoveプロパティとして公開するクラス。
/// </summary>
public class Controller : MonoBehaviour
{
    public InputSystemPlayer inputActions;

    private void Start()
    {
        inputActions = new InputSystemPlayer();
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        Debug.Log("Movement Input: " + movement);
        Move = movement;
    }
    public Vector2 Move { get; private set; }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Disable();
    }
}

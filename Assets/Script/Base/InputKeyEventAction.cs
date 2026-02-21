using UnityEngine;
using UnityEngine.Events;

public class InputKeyEventAction : MonoBehaviour
{
    [System.Serializable]
    public class KeyAction
    {
        public KeyCode key;
        [SerializeField] UnityEvent actionEvent;
        public void Invoke()
        {
            actionEvent.Invoke();
        }
    }

    [SerializeField] KeyAction[] actions;
    private void Update()
    {
        foreach (var action in actions)
        {
            if (Input.GetKeyDown(action.key))
            {
                action.Invoke();
            }
        }
    }
}

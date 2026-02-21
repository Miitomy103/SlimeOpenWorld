using UnityEngine;
using UnityEngine.Events;

public class OnTriggerAction : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnterEvent;
    [SerializeField] UnityEvent onTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExitEvent.Invoke();
    }
}

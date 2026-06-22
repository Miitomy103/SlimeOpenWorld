using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// OnTriggerEnter/Exitが発生したときに、Inspectorで設定したUnityEventを発火させるコンポーネント。
/// </summary>
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

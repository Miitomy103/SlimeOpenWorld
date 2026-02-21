using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    [SerializeField] CursorLock cursorLock;

    private void Awake()
    {
        if (cursorLock == null)
        {
            cursorLock = FindObjectOfType<CursorLock>();
        }
    }
}

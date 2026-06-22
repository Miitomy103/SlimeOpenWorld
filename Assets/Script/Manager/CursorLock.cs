using UnityEngine;

/// <summary>
/// マウスカーソルの表示/ロック状態を切り替えるユーティリティ。
/// </summary>
public class CursorLock : MonoBehaviour
{
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //}

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1f;
        }
    }

    public void ChangeLock(bool isLock)
    {
        if (isLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
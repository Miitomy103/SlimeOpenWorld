using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{

    [Header("Manager")]
    [SerializeField] RespawnManager respawnManager;

    [Header("Die")]
    [SerializeField] float dieFadeTime = 2f;
    [SerializeField] GameObject dieUI;
    [SerializeField] Graphic[] graphics;

    private void Start()
    {
        Debug.Log(Time.timeScale);
        GameStart();
    }
    private void GameStart()
    {
        SaveManager.Instance.Initialize();
    }
    public void Die()
    {
        dieUI.SetActive(true);
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        float time = 0f;
        Color[] originalColors = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; i++)
        {
            originalColors[i] = graphics[i].color;
        }
        while (time < dieFadeTime)
        {
            time += Time.unscaledDeltaTime;
            float alpha = time / dieFadeTime;
            float timeScale;
            if (alpha < 1f) timeScale = Mathf.Lerp(1f, 0.1f, alpha);
            else timeScale = 0.1f;
            Time.timeScale = timeScale;
            for (int i = 0; i < graphics.Length; i++)
            {
                Color newColor = originalColors[i];
                newColor.a = Mathf.Lerp(originalColors[i].a, 1f, alpha);
                graphics[i].color = newColor;
            }
            yield return null;
        }
        for (int i = 0; i < graphics.Length; i++)
        {
            Color newColor = originalColors[i];
            newColor.a = 1f;
            graphics[i].color = newColor;
        }
        PlayerInput playerInput = InputData.Instance.InputAction();
        //yield return new WaitUntil(() => playerInput.Action0.onDown);
        Debug.Log("Respawn");
        Time.timeScale = 1f;
        dieUI.SetActive(false);
        respawnManager.Respawn();
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif

    }
}

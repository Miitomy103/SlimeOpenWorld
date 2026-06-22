using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    public Vector3 playerPos { get; private set; }
    public HostBase playerHost { get; private set; }

    public event System.Action OnSceneLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, Vector3 pos)
    {
        if (sceneName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            return;
        }

        OnSceneLoaded?.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        playerHost = PlayerController.Instance.HostBase;
        DontDestroyOnLoad(playerHost.gameObject);

        playerPos = pos;
    }

    // ホスト引き継ぎ後にクリアする（二重適用防止）
    public void ClearPlayerHost()
    {
        playerHost = null;
    }
}
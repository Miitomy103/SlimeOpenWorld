using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    SaveObject<PlayerSaveData> playerSaveObject;
    SaveObject<QuestSaveData> questSaveObject;

#if UNITY_EDITOR
    [SerializeField] bool loadOnStart = true;
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance != this) return;
        Debug.Log("Initializing SaveManager...");

        // セーブデータがあれば上書き適用
        PlayerLoad();
        QuestLoad();
    }

    void PlayerLoad()
    {
#if UNITY_EDITOR
        if (!loadOnStart)
        {
            Debug.Log("Skipping player load on start (editor setting)");
            return;
        }
#endif
        playerSaveObject = new SaveObject<PlayerSaveData>("player_save");
        var data = playerSaveObject.Get();
        if (data != null)
        {
            PlayerController.Instance.ApplySaveData(data);
        }
    }

    void QuestLoad()
    {
        questSaveObject = new SaveObject<QuestSaveData>("quest_save");
        var data = questSaveObject.Get();
        if (data != null)
        {
            QuestManager.Instance.LoadSaveData(data);
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Debug.Log("Saving data...");
        PlayerSave();
        QuestSave();
    }

    void PlayerSave()
    {
        PlayerSaveData data = new PlayerSaveData(PlayerController.Instance.HostBase);
        playerSaveObject.Save(data);
    }

    void QuestSave()
    {
        QuestSaveData data = QuestManager.Instance.GetSaveData();
        questSaveObject.Save(data);
    }
}
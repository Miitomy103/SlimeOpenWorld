using UnityEngine;

/// <summary>
/// データをロード、セーブするクラス
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    SaveObject<PlayerSaveData> playerSaveObject;
    SaveObject<QuestSaveData> questSaveObject;

    [SerializeField] bool loadOnStart = true;
    [SerializeField] bool isDefaultScene = true;
    const string defaultDataPath = "DefaultData/";

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

    public void Initialize()
    {
        if (Instance != this) return;
        Debug.Log("Initializing SaveManager...");

        // セーブデータがあれば上書き適用
        PlayerLoad();
        QuestLoad();
        if(isDefaultScene)
            SceneController.Instance.OnSceneLoaded += () =>Save();
    }

    /// <summary>
    /// プレイヤーの位置などのデータをロードする
    /// </summary>
    void PlayerLoad()
    {
        if (!loadOnStart)
        {
            Debug.Log("Skipping player load on start (editor setting)");
            PlayerController.Instance.Initialize();
            return;
        }
        playerSaveObject = new SaveObject<PlayerSaveData>("player_save");
        var data = playerSaveObject.Get();
        if (data != null)
        {
            PlayerController.Instance.ApplySaveData(data);
        }
        else
        {
            var defaultData = Resources.Load<TextAsset>($"{defaultDataPath}player_save");
            if (defaultData != null)
            {
                PlayerSaveData defaultSaveData = JsonUtility.FromJson<PlayerSaveData>(defaultData.text);
                PlayerController.Instance.ApplySaveData(defaultSaveData);
            }
            else
            {
                Debug.LogWarning("Default player save data not found in Resources: " + defaultDataPath + "player_save");
            }
        }
    }

    /// <summary>
    /// クエストの進行状況などのデータをロードする
    /// </summary>
    void QuestLoad()
    {
        questSaveObject = new SaveObject<QuestSaveData>("quest_save");
        var data = questSaveObject.Get();
        if (data != null)
        {
            QuestManager.Instance.LoadSaveData(data);
        }
        else
        {
            var defaultData = Resources.Load<TextAsset>($"{defaultDataPath}quest_save");
            if(defaultData != null)
            {
                QuestSaveData defaultSaveData = JsonUtility.FromJson<QuestSaveData>(defaultData.text);
                QuestManager.Instance.LoadSaveData(defaultSaveData);
            }
            else
            {
                Debug.LogWarning("Default quest save data not found in Resources: " + defaultDataPath + "quest_save");
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Debug.Log("Saving data...");
        PlayerSave();
        QuestSave();
    }

    /// <summary>
    /// プレイヤーの位置などのデータをセーブする
    /// </summary>
    void PlayerSave()
    {
        PlayerSaveData data = new PlayerSaveData(PlayerController.Instance.HostBase);
        playerSaveObject.Save(data);
    }

    /// <summary>
    /// クエストの進行状況などのデータをセーブする
    /// </summary>
    void QuestSave()
    {
        QuestSaveData data = QuestManager.Instance.GetSaveData();
        questSaveObject.Save(data);
    }
}
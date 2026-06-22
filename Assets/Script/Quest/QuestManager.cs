// ================================================================================
// QuestManager.cs - クエスト管理システム
// ================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    // クエスト管理用コレクション
    private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();
    private List<Quest> activeQuests = new List<Quest>();
    private HashSet<string> completedQuestIDs = new HashSet<string>();
    private List<Quest> availableQuests = new List<Quest>();

    // イベント
    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest> OnQuestFailed;
    public event Action<Quest, Objective> OnObjectiveUpdated;
    public event Action OnQuestsUpdated;

    //クエスト必要数管理
    private Dictionary<string, int> killCounts = new Dictionary<string, int>();
    private Dictionary<string, int> interactCounts = new Dictionary<string, int>();

    [Header("設定")]
    [SerializeField] private bool loadQuestsOnStart = true;
    [SerializeField] private string questResourcePath = "Quests";

    [Header("Debug")]
    [SerializeField,ReadOnly] string debugCurrentQuestID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnQuestStarted += (quest) => Debug.Log($"[Event] Quest Started: {quest.data.questName}");
        OnQuestCompleted += (quest) => Debug.Log($"[Event] Quest Completed: {quest.data.questName}");
        OnQuestFailed += (quest) => Debug.Log($"[Event] Quest Failed: {quest.data.questName}");
        OnObjectiveUpdated += (quest, objective) => Debug.Log($"[Event] Objective Updated: {objective.data.description})");
        OnQuestsUpdated += () => Debug.Log($"[Event] Quests Updated: {activeQuests.Count} active, {completedQuestIDs.Count} completed, {availableQuests.Count} available");
        StartQuest("quest_001");

    }
    private void Initialize()
    {
        if (loadQuestsOnStart)
        {
            LoadAllQuestData();
        }

        SubscribeToGameEvents();
    }
    private void Update()
    {
        // 時間制限付きクエストのタイマー更新
        UpdateQuestTimers();
    }

    private void OnDestroy()
    {
        UnsubscribeFromGameEvents();
    }

    // ================================================================================
    // クエストデータの読み込み
    // ================================================================================

    private void LoadAllQuestData()
    {
        QuestData[] questDataArray = Resources.LoadAll<QuestData>(questResourcePath);

        if (questDataArray.Length == 0)
        {
            Debug.LogWarning($"No quest data found in Resources/{questResourcePath}");
            return;
        }

        foreach (var questData in questDataArray)
        {
            if (string.IsNullOrEmpty(questData.questID))
            {
                Debug.LogError($"Quest {questData.name} has no ID assigned!");
                continue;
            }

            Quest quest = new Quest(questData);
            allQuests.Add(questData.questID, quest);
        }

        UpdateAvailableQuests();
        Debug.Log($"Loaded {allQuests.Count} quests");
    }

    public void RegisterQuest(QuestData questData)
    {
        if (!allQuests.ContainsKey(questData.questID))
        {
            Quest quest = new Quest(questData);
            allQuests.Add(questData.questID, quest);
            UpdateAvailableQuests();
        }
    }

    // ================================================================================
    // クエスト開始・完了・失敗
    // ================================================================================

    /// <summary>
    /// 前提条件・重複・リピート可否をチェックした上でクエストを開始する。
    /// </summary>
    public bool StartQuest(string questID)
    {
        if (!allQuests.ContainsKey(questID))
        {
            Debug.LogError($"Quest {questID} not found!");
            return false;
        }

        Quest quest = allQuests[questID];

        // 前提条件チェック
        if (!CanStartQuest(quest))
        {
            Debug.Log($"Prerequisites not met for quest {questID}");
            return false;
        }

        // すでにアクティブまたは完了済みかチェック
        if (activeQuests.Contains(quest))
        {
            Debug.Log($"Quest {questID} is already active");
            return false;
        }

        if (completedQuestIDs.Contains(questID) && !quest.data.isRepeatable)
        {
            Debug.Log($"Quest {questID} is already completed and not repeatable");
            return false;
        }

        // クエスト開始
        quest.state = QuestState.Active;
        quest.InitializeObjectives();
        quest.remainingTime = quest.data.timeLimit;

        // イベント購読
        quest.OnObjectiveCompleted += HandleObjectiveCompleted;

        activeQuests.Add(quest);
        availableQuests.Remove(quest);

        OnQuestStarted?.Invoke(quest);
        OnQuestsUpdated?.Invoke();

        debugCurrentQuestID = questID;

        Debug.Log($"Quest started: {quest.data.questName}");
        return true;
    }

    /// <summary>
    /// クエスト開始前提条件チェック
    /// </summary>
    private bool CanStartQuest(Quest quest)
    {
        // 前提クエストチェック
        if (quest.data.prerequisiteQuestIDs != null && quest.data.prerequisiteQuestIDs.Count > 0)
        {
            foreach (var prereqID in quest.data.prerequisiteQuestIDs)
            {
                if (!completedQuestIDs.Contains(prereqID))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// クエスト完了処理
    /// </summary>
    private void CompleteQuest(Quest quest)
    {
        quest.state = QuestState.Completed;
        activeQuests.Remove(quest);
        completedQuestIDs.Add(quest.data.questID);

        // イベント購読解除
        quest.OnObjectiveCompleted -= HandleObjectiveCompleted;

        // 報酬付与
        GiveRewards(quest);

        OnQuestCompleted?.Invoke(quest);
        OnQuestsUpdated?.Invoke();

        // 新たに受注可能になったクエストを更新
        UpdateAvailableQuests();

        Debug.Log($"Quest completed: {quest.data.questName}");
    }

    /// <summary>
    /// クエスト放棄処理
    /// </summary>
    public void AbandonQuest(string questID)
    {
        Quest quest = GetActiveQuest(questID);
        if (quest != null)
        {
            quest.state = QuestState.NotStarted;
            activeQuests.Remove(quest);
            quest.OnObjectiveCompleted -= HandleObjectiveCompleted;

            UpdateAvailableQuests();
            OnQuestsUpdated?.Invoke();

            Debug.Log($"Quest abandoned: {quest.data.questName}");
        }
    }

    /// <summary>
    /// クエスト失敗処理
    /// </summary>
    private void FailQuest(Quest quest)
    {
        quest.Fail();
        activeQuests.Remove(quest);
        quest.OnObjectiveCompleted -= HandleObjectiveCompleted;

        OnQuestFailed?.Invoke(quest);
        OnQuestsUpdated?.Invoke();

        UpdateAvailableQuests();

        Debug.Log($"Quest failed: {quest.data.questName}");
    }

    // ================================================================================
    // 報酬システム
    // ================================================================================

    private void GiveRewards(Quest quest)
    {
        if (quest.data.reward == null) return;

        QuestReward reward = quest.data.reward;

        // 経験値
        if (reward.experience > 0 && PlayerController.Instance != null)
        {
            PlayerController.Instance.AddExperience(reward.experience);
            Debug.Log($"Gained {reward.experience} XP");
        }

        // お金
        if (reward.gold > 0 && PlayerController.Instance != null)
        {
            PlayerController.Instance.AddGold(reward.gold);
            Debug.Log($"Gained {reward.gold} gold");
        }

        // アイテム
        if (reward.items != null && reward.items.Count > 0 && InventoryManager.Instance != null)
        {
            foreach (var item in reward.items)
            {
                InventoryManager.Instance.AddItem(item.itemID, item.quantity);
                Debug.Log($"Gained {item.quantity}x {item.itemID}");
            }
        }
    }

    // ================================================================================
    // ゲームイベントの購読
    // ================================================================================

    private void SubscribeToGameEvents()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnItemCollected += HandleItemCollected;
        GameEvents.OnLocationReached += HandleLocationReached;
        GameEvents.OnNPCInteracted += HandleNPCInteraction;
        GameEvents.OnObjectInteracted += HandleObjectInteraction;
        GameEvents.OnPossessionChanged += HandlePossessChanged;
    }

    private void UnsubscribeFromGameEvents()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnItemCollected -= HandleItemCollected;
        GameEvents.OnLocationReached -= HandleLocationReached;
        GameEvents.OnNPCInteracted -= HandleNPCInteraction;
        GameEvents.OnObjectInteracted -= HandleObjectInteraction;
        GameEvents.OnPossessionChanged -= HandlePossessChanged;
    }

    private void HandleEnemyKilled(string enemyID)
    {
        if(!killCounts.ContainsKey(enemyID))
        {
            killCounts[enemyID] = 0;
        }
        killCounts[enemyID]++;
        UpdateQuestProgress(ObjectiveType.Kill, enemyID, killCounts[enemyID]);
    }

    private void HandleItemCollected(string itemID, int amount)
    {
        UpdateQuestProgress(ObjectiveType.Collect, itemID, amount);
    }

    private void HandleLocationReached(string locationID)
    {
        UpdateQuestProgress(ObjectiveType.Reach, locationID, 1);
    }

    private void HandleNPCInteraction(string npcID)
    {
        UpdateQuestProgress(ObjectiveType.Talk, npcID, 1);
    }

    private void HandleObjectInteraction(string objectID)
    {
        if(!interactCounts.ContainsKey(objectID))
        {
            interactCounts[objectID] = 0;
        }
        interactCounts[objectID]++;
        UpdateQuestProgress(ObjectiveType.Interact, objectID, interactCounts[objectID]);
    }

    private void HandlePossessChanged(string hostedID)
    {
        UpdateQuestProgress(ObjectiveType.Possess, hostedID, 1);
    }

    private void UpdateQuestProgress(ObjectiveType type, string targetID, int amount)
    {
        foreach (var quest in activeQuests.ToList()) // ToListで反復中の変更に対応
        {
            quest.UpdateProgress(type, targetID, amount);
            CheckQuestCompletion(quest);
        }
    }

    private void HandleObjectiveCompleted(Quest quest, Objective objective)
    {
        OnObjectiveUpdated?.Invoke(quest, objective);
        Debug.Log($"Objective completed: {objective.data.description}");
    }

    private void CheckQuestCompletion(Quest quest)
    {
        if (quest.IsCompleted())
        {
            CompleteQuest(quest);
        }
    }

    // ================================================================================
    // タイマー管理
    // ================================================================================

    private void UpdateQuestTimers()
    {
        foreach (var quest in activeQuests.ToList())
        {
            if (quest.data.timeLimit > 0)
            {
                quest.UpdateTimer(Time.deltaTime);

                if (quest.IsFailed())
                {
                    FailQuest(quest);
                }
            }
        }
    }

    // ================================================================================
    // 受注可能クエスト管理
    // ================================================================================

    private void UpdateAvailableQuests()
    {
        availableQuests.Clear();

        foreach (var quest in allQuests.Values)
        {
            // すでに完了済み（かつリピート不可）またはアクティブならスキップ
            if ((completedQuestIDs.Contains(quest.data.questID) && !quest.data.isRepeatable) ||
                activeQuests.Contains(quest))
            {
                continue;
            }

            // 前提条件を満たしているか確認
            if (CanStartQuest(quest))
            {
                availableQuests.Add(quest);
            }
        }
    }

    // ================================================================================
    // 公開API - クエスト取得
    // ================================================================================

    public List<Quest> GetAvailableQuests()
    {
        return new List<Quest>(availableQuests);
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(activeQuests);
    }

    public List<Quest> GetCompletedQuests()
    {
        return allQuests.Values
            .Where(q => completedQuestIDs.Contains(q.data.questID))
            .ToList();
    }

    public Quest GetQuestByID(string questID)
    {
        return allQuests.ContainsKey(questID) ? allQuests[questID] : null;
    }

    public Quest GetActiveQuest(string questID)
    {
        return activeQuests.Find(q => q.data.questID == questID);
    }

    public bool IsQuestActive(string questID)
    {
        return activeQuests.Any(q => q.data.questID == questID);
    }

    public bool IsQuestCompleted(string questID)
    {
        return completedQuestIDs.Contains(questID);
    }

    public List<Quest> GetQuestsForNPC(string npcID)
    {
        return availableQuests.FindAll(q => q.data.QuestGiverID == npcID);
    }

    // ================================================================================
    // セーブ・ロード
    // ================================================================================


    public QuestSaveData GetSaveData()
    {
        QuestSaveData saveData = new QuestSaveData
        {
            activeQuestIDs = activeQuests.Select(q => q.data.questID).ToList(),
            completedQuestIDs = completedQuestIDs.ToList(),
            objectiveProgresses = new List<ObjectiveProgress>(),
            questTimers = new List<QuestTimeData>()
        };

        // 各クエストの進行状況を保存
        foreach (var quest in activeQuests)
        {
            for (int i = 0; i < quest.objectives.Count; i++)
            {
                saveData.objectiveProgresses.Add(new ObjectiveProgress
                {
                    questID = quest.data.questID,
                    objectiveIndex = i,
                    currentAmount = quest.objectives[i].currentAmount
                });
            }

            // タイマー情報を保存
            if (quest.data.timeLimit > 0)
            {
                saveData.questTimers.Add(new QuestTimeData
                {
                    questID = quest.data.questID,
                    remainingTime = quest.remainingTime
                });
            }
        }

        return saveData;
    }

    public void LoadSaveData(QuestSaveData saveData)
    {
        if (saveData == null)
        {
            Debug.LogWarning("Quest save data is null");
            return;
        }

        // 完了済みクエストを復元
        completedQuestIDs = new HashSet<string>(saveData.completedQuestIDs);

        // アクティブなクエストを復元
        activeQuests.Clear();
        foreach (var questID in saveData.activeQuestIDs)
        {
            if (allQuests.ContainsKey(questID))
            {
                Quest quest = allQuests[questID];
                quest.state = QuestState.Active;
                quest.InitializeObjectives();
                quest.OnObjectiveCompleted += HandleObjectiveCompleted;
                activeQuests.Add(quest);
            }
        }

        // 進行状況を復元
        foreach (var progress in saveData.objectiveProgresses)
        {
            Quest quest = GetActiveQuest(progress.questID);
            if (quest != null && progress.objectiveIndex < quest.objectives.Count)
            {
                quest.objectives[progress.objectiveIndex].SetProgress(progress.currentAmount);
            }
        }

        // タイマー情報を復元
        foreach (var timerData in saveData.questTimers)
        {
            Quest quest = GetActiveQuest(timerData.questID);
            if (quest != null)
            {
                quest.remainingTime = timerData.remainingTime;
            }
        }

        UpdateAvailableQuests();
        OnQuestsUpdated?.Invoke();

        Debug.Log($"Loaded quest data: {activeQuests.Count} active, {completedQuestIDs.Count} completed");
    }

    // ================================================================================
    // デバッグ機能
    // ================================================================================

    public void ForceCompleteQuest(string questID)
    {
        Quest quest = GetActiveQuest(questID);
        if (quest != null)
        {
            CompleteQuest(quest);
        }
    }

    [ContextMenu("Reset All Quests")]
    public void ResetAllQuests()
    {
        activeQuests.Clear();
        completedQuestIDs.Clear();

        foreach (var quest in allQuests.Values)
        {
            quest.state = QuestState.NotStarted;
        }

        UpdateAvailableQuests();
        OnQuestsUpdated?.Invoke();

        Debug.Log("All quests reset");
    }

    public void DebugPrintQuestStatus()
    {
        Debug.Log("=== Quest System Status ===");
        Debug.Log($"Total Quests: {allQuests.Count}");
        Debug.Log($"Active Quests: {activeQuests.Count}");
        Debug.Log($"Completed Quests: {completedQuestIDs.Count}");
        Debug.Log($"Available Quests: {availableQuests.Count}");

        Debug.Log("\nActive Quests:");
        foreach (var quest in activeQuests)
        {
            Debug.Log($"  - {quest.data.questName}");
            foreach (var obj in quest.objectives)
            {
                Debug.Log($"    * {obj.data.description}: {obj.currentAmount}/{obj.data.requiredAmount}");
            }
        }
    }
}
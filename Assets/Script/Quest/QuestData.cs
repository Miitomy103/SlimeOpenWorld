// ================================================================================
// QuestData.cs - クエストデータ定義（ScriptableObject）
// ================================================================================
// ================================================================================
// QuestData.cs - クエストデータ定義（ScriptableObject）with Tooltips
// ================================================================================
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest System/Quest")]
public class QuestData : ScriptableObject
{
    [Header("基本情報")]
    [Tooltip("クエストの一意な識別子。他のクエストと重複しないようにしてください（例: quest_001）")]
    public string questID;

    [Tooltip("プレイヤーに表示されるクエスト名")]
    public string questName;

    [Tooltip("クエストの詳細説明。プレイヤーに目的を伝えます")]
    [TextArea(3, 5)]
    public string description;

    [Tooltip("クエストログやUIに表示されるアイコン画像")]
    public Sprite questIcon;

    [Header("クエスト設定")]
    [Tooltip("このクエストを受注するために必要なプレイヤーレベル。0以下の場合は制限なし")]
    public int requiredLevel = 1;

    [Tooltip("このクエストを提供するNPCのID。NPCとの紐付けに使用します")]
    [SerializeField] string questGiverID;
    public string QuestGiverID => questGiverID;

    [Tooltip("このクエストを受注する前に完了している必要があるクエストIDのリスト")]
    public List<string> prerequisiteQuestIDs = new List<string>();

    [Header("目標")]
    [Tooltip("クエストで達成すべき目標のリスト。すべての必須目標を完了するとクエストが完了します")]
    public List<ObjectiveData> objectives = new List<ObjectiveData>();

    [Header("報酬")]
    [Tooltip("クエスト完了時にプレイヤーが獲得する報酬")]
    public QuestReward reward;

    [Header("オプション")]
    [Tooltip("このクエストを繰り返し受注できるかどうか")]
    public bool isRepeatable = false;

    [Tooltip("クエストの制限時間（秒）。0の場合は時間制限なし")]
    public float timeLimit = 0f;
}
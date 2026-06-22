// ================================================================================
// QuestSaveData.cs - セーブデータ構造
// ================================================================================
using System;
using System.Collections.Generic;

[Serializable]
public class QuestSaveData
{
    /// <summary>
    /// 現在アクティブなクエストのIDリストと、完了したクエストのIDリストを保持します。
    /// </summary>
    public List<string> activeQuestIDs = new List<string>();
    /// <summary>
    /// 完了したクエストのIDリストを保持します。
    /// </summary>
    public List<string> completedQuestIDs = new List<string>();
    /// <summary>
    /// クエストの目標の進行状況を保持します。クエストID、目標インデックス、現在の達成量を保存します。
    /// </summary>
    public List<ObjectiveProgress> objectiveProgresses = new List<ObjectiveProgress>();
    /// <summary>
    /// クエストのタイマー情報を保持します。クエストIDと残り時間を保存します。
    /// </summary>
    public List<QuestTimeData> questTimers = new List<QuestTimeData>();
}

[Serializable]
public class ObjectiveProgress
{
    public string questID;
    public int objectiveIndex;
    public int currentAmount;
}

[Serializable]
public class QuestTimeData
{
    public string questID;
    public float remainingTime;
}
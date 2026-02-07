// ================================================================================
// QuestSaveData.cs - セーブデータ構造
// ================================================================================
using System;
using System.Collections.Generic;

[Serializable]
public class QuestSaveData
{
    public List<string> activeQuestIDs = new List<string>();
    public List<string> completedQuestIDs = new List<string>();
    public List<ObjectiveProgress> objectiveProgresses = new List<ObjectiveProgress>();
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
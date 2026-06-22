using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// ================================================================================
// Quest.cs - クエストの実行時クラス
// ================================================================================

public class Quest
{
    public QuestData data;
    public QuestState state;
    public List<Objective> objectives;
    public float remainingTime;

    public event Action<Quest> OnQuestStateChanged;
    public event Action<Quest, Objective> OnObjectiveCompleted;

    public Quest(QuestData data)
    {
        this.data = data;
        this.state = QuestState.NotStarted;
        this.objectives = new List<Objective>();
        this.remainingTime = data.timeLimit;
    }

    public void InitializeObjectives()
    {
        objectives.Clear();

        foreach (var objectiveData in data.objectives)
        {
            Objective objective = new Objective(objectiveData);
            objective.OnCompleted += (obj) => OnObjectiveCompleted?.Invoke(this, obj);
            objectives.Add(objective);
        }
    }

    public void UpdateProgress(ObjectiveType type, string targetID, int amount)
    {
        foreach (var objective in objectives)
        {
            if (objective.data.type == type &&
                objective.data.targetID == targetID &&
                !objective.isCompleted)
            {
                objective.UpdateProgress(amount);
            }
        }
    }

    public bool IsCompleted()
    {
        // 必須目標がすべて完了しているか確認
        return objectives
            .Where(obj => !obj.data.isOptional)
            .All(obj => obj.isCompleted);
    }

    public bool IsFailed()
    {
        return state == QuestState.Failed;
    }

    public void Fail()
    {
        state = QuestState.Failed;
        OnQuestStateChanged?.Invoke(this);
    }

    public void UpdateTimer(float deltaTime)
    {
        if (data.timeLimit > 0 && state == QuestState.Active)
        {
            remainingTime -= deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                Fail();
            }
        }
    }
}


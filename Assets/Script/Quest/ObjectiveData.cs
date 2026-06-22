using UnityEngine;
using System;
using System.Collections.Generic;

// ================================================================================
// ObjectiveData.cs - 目標データ定義 with Tooltips
// ================================================================================

[Serializable]
public class ObjectiveData
{
    [Tooltip("目標のタイプ（Kill: 敵を倒す、Collect: アイテム収集、Reach: 場所到達など）")]
    public ObjectiveType type;

    [Tooltip("目標の対象ID（敵ID、アイテムID、場所IDなど。typeに応じて設定）")]
    public string targetID;

    [Tooltip("目標達成に必要な数量（敵を10体倒す、アイテムを5個集めるなど）")]
    public int requiredAmount = 1;

    [Tooltip("プレイヤーに表示される目標の説明文")]
    public string description;

    [Tooltip("オプション目標の場合はtrue。オプション目標は完了しなくてもクエストをクリアできます")]
    public bool isOptional = false;
}

// ================================================================================
// Objective.cs - 目標の実行時クラス
// ================================================================================

public class Objective
{
    public ObjectiveData data;
    public int currentAmount;
    public bool isCompleted;

    public event Action<Objective> OnProgressUpdated;
    public event Action<Objective> OnCompleted;

    public Objective(ObjectiveData data)
    {
        this.data = data;
        this.currentAmount = 0;
        this.isCompleted = false;
    }

    public void UpdateProgress(int amount)
    {
        if (isCompleted) return;

        currentAmount += amount;

        if (currentAmount > data.requiredAmount)
        {
            currentAmount = data.requiredAmount;
        }

        OnProgressUpdated?.Invoke(this);

        if (currentAmount >= data.requiredAmount)
        {
            Complete();
        }
    }

    public void SetProgress(int amount)
    {
        currentAmount = amount;
        if (currentAmount >= data.requiredAmount && !isCompleted)
        {
            Complete();
        }
    }

    private void Complete()
    {
        isCompleted = true;
        OnCompleted?.Invoke(this);
    }

    public float GetProgressPercentage()
    {
        return (float)currentAmount / data.requiredAmount;
    }
}

[Serializable]
public class QuestReward
{
    public int experience;
    public int gold;
    public List<ItemReward> items = new List<ItemReward>();
}
[Serializable]
public class ItemReward
{
    [Tooltip("報酬として与えるアイテムのID")]
    public string itemID;

    [Tooltip("報酬として与えるアイテムの個数")]
    public int quantity;
}
public enum QuestState
{
    NotStarted,
    Active,
    Completed,
    Failed
}

public enum ObjectiveType
{
    Kill,           // 敵を倒す
    Collect,        // アイテム収集
    Reach,          // 場所に到達
    Interact,       // オブジェクトと相互作用
    Escort,         // 護衛
    Deliver,        // 配達
    Talk,           // NPCと会話
    Possess,        // 対象を憑依
}
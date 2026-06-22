using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestView : MonoBehaviour
{
    [SerializeField] private GameObject uiParent;
    [SerializeField] private Text questObjectiveText;

    List<string> activeArrows = new List<string>();

    IPossess[] possessTargets;
    IEnemy[] enemies;
    ILocation[] locations;
    private void Awake()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestStarted += OnQuestStarted;
            QuestManager.Instance.OnObjectiveUpdated += OnObjectiveUpdated;
            QuestManager.Instance.OnQuestCompleted += OnQuestCompleted;
        }

        possessTargets = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IPossess>()
            .ToArray();
        enemies = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IEnemy>()
            .ToArray();
        locations = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ILocation>()
            .ToArray();
        // 初期状態は非表示
        //if (uiParent != null)
        //{
        //    uiParent.SetActive(false);
        //}
    }

    private void OnDestroy()
    {
        // イベント購読解除（メモリリーク防止）
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestStarted -= OnQuestStarted;
            QuestManager.Instance.OnObjectiveUpdated -= OnObjectiveUpdated;
            QuestManager.Instance.OnQuestCompleted -= OnQuestCompleted;
        }
    }

    private void OnQuestStarted(Quest quest)
    {
        if (uiParent != null)
        {
            uiParent.SetActive(true);
        }

        // クエスト開始時は最初の目標（index 0）を表示
        if (questObjectiveText != null && quest != null && quest.objectives.Count > 0)
        {
            Objective firstObjective = quest.objectives[0];
            questObjectiveText.text = $"{firstObjective.data.description}";
            UpdateArrow(firstObjective);
        }
    }

    private void OnObjectiveUpdated(Quest quest, Objective objective)
    {
        // 現在の進行中目標を更新
        UpdateObjectiveDisplay(quest);
    }

    private void OnQuestCompleted(Quest quest)
    {
        // 他にアクティブなクエストがあるかチェック
        var activeQuests = QuestManager.Instance.GetActiveQuests();

        if (activeQuests.Count > 0)
        {
            // 次のクエストの最初の目標を表示
            Quest nextQuest = activeQuests[0];
            if (nextQuest.objectives.Count > 0)
            {
                Objective firstObjective = nextQuest.objectives[0];
                questObjectiveText.text = $"{firstObjective.data.description}";
            }
        }
        else
        {
            // アクティブなクエストがなければ非表示
            if (uiParent != null)
            {
                uiParent.SetActive(false);
            }
        }


    }

    private void UpdateObjectiveDisplay(Quest quest)
    {
        if (quest == null || questObjectiveText == null) return;

        // 最初の未完了目標を探す
        Objective currentObjective = null;
        foreach (var objective in quest.objectives)
        {
            if (!objective.isCompleted)
            {
                currentObjective = objective;
                break;
            }
            else
            {
                // 完了した目標の矢印を削除
                if (activeArrows.Contains(objective.data.targetID))
                {
                    UIManager.Instance.RemoveArrow(objective.data.targetID);
                    activeArrows.Remove(objective.data.targetID);
                }
            }
        }

        if (currentObjective != null)
        {
            Debug.Log($"Updating objective display: {currentObjective.data.description} (Progress: {currentObjective.currentAmount}/{currentObjective.data.requiredAmount})");
            // 進行状況と説明を表示
            questObjectiveText.text = $"{currentObjective.data.description}";
            if (currentObjective.data.requiredAmount > 1)
            {
                questObjectiveText.text += $" ({currentObjective.currentAmount}/{currentObjective.data.requiredAmount})";

                currentObjective.OnProgressUpdated += (obj) =>
                {
                    if (obj == currentObjective)
                    {
                        AmountText(obj);
                    }
                };
            }

            UpdateArrow(currentObjective);
        }
        else
        {
            // すべて完了している場合
            questObjectiveText.text = "目標達成！クエストを完了してください";
        }
    }

    private void AmountText(Objective objective)
    {
        questObjectiveText.text = $"{objective.data.description} ({objective.currentAmount}/{objective.data.requiredAmount})";
    }

    private void UpdateArrow(Objective objective)
    {
        if (activeArrows.Contains(objective.data.targetID))
        {
            UIManager.Instance.RemoveArrow(objective.data.targetID);
            activeArrows.Remove(objective.data.targetID);
        }

        Debug.Log($"Updating arrow for objective: {objective.data.description} (Type: {objective.data.type}, TargetID: {objective.data.targetID})");
        if (objective.data.type==ObjectiveType.Possess)
        {
            var possessors = possessTargets;
            foreach (var possessor in possessors)
            {
                if (possessor.PossessId==objective.data.targetID)
                {
                    UIManager.Instance.CreateArrow(objective.data.targetID, possessor.Transform);
                    activeArrows.Add(objective.data.targetID);
                }
            }
        }
        else if (objective.data.type == ObjectiveType.Kill)
        {
            var killTargets = enemies;
            foreach (var target in killTargets)
            {
                if (target.EnemyId == objective.data.targetID)
                {
                    UIManager.Instance.CreateArrow(objective.data.targetID, target.Transform);
                    activeArrows.Add(objective.data.targetID);
                }
            }
        }
        else if(objective.data.type == ObjectiveType.Reach)
        {
            var location = locations;
            foreach (var loc in location)
            {
                if (loc.LocationID == objective.data.targetID)
                {
                    UIManager.Instance.CreateArrow(objective.data.targetID, loc.Transform);
                    activeArrows.Add(objective.data.targetID);
                }
            }
        }
    }
}
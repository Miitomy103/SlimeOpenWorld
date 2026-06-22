using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : MonoBehaviour,IInteractable
{
    [SerializeField] private string npcID;

    private List<Quest> availableQuests = new List<Quest>();

    public bool CanInteract => true;

    public string GetInteractText => npcID;

    [SerializeField] private string interactableID;
    public string InteractableID => interactableID;

    private void Start()
    {
        UpdateQuestMarker();

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestsUpdated += UpdateQuestMarker;
        }
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestsUpdated -= UpdateQuestMarker;
        }
    }

    public void Interact(GameObject player)
    {
        GameEvents.NPCInteracted(npcID);

        // クエストUIを表示
        ShowQuestUI();
    }

    private void UpdateQuestMarker()
    {
        if (QuestManager.Instance == null) return;

        availableQuests = QuestManager.Instance.GetQuestsForNPC(npcID);
    }

    private void ShowQuestUI()
    {
        // ここでクエストUIを表示する処理を実装
        Debug.Log($"Interacting with NPC: {npcID}");
        Debug.Log($"Available quests: {availableQuests.Count}");

        foreach (var quest in availableQuests)
        {
            Debug.Log($"  - {quest.data.questName}");
        }
    }

}

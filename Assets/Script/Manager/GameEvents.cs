// ================================================================================
// GameEvents.cs - ゲームイベントシステム
// ================================================================================
using System;

public static class GameEvents
{
    public static event Action<string> OnEnemyKilled;
    public static event Action<string, int> OnItemCollected;
    public static event Action<string> OnLocationReached;
    public static event Action<string> OnNPCInteracted;
    public static event Action<string> OnObjectInteracted;
    public static event Action<string> OnPossessionChanged;

    public static void EnemyKilled(string enemyID)
    {
        OnEnemyKilled?.Invoke(enemyID);
    }

    public static void ItemCollected(string itemID, int amount = 1)
    {
        OnItemCollected?.Invoke(itemID, amount);
    }

    public static void LocationReached(string locationID)
    {
        OnLocationReached?.Invoke(locationID);
    }

    public static void NPCInteracted(string npcID)
    {
        OnNPCInteracted?.Invoke(npcID);
    }

    public static void ObjectInteracted(string objectID)
    {
        OnObjectInteracted?.Invoke(objectID);
    }
    public static void PossessionChanged(string newHostID)
    {
        OnPossessionChanged?.Invoke(newHostID);
    }
}
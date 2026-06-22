// ================================================================================
// InventoryManager.cs - インベントリ管理（簡易版）
// ================================================================================
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemID, int quantity)
    {
        if (items.ContainsKey(itemID))
        {
            items[itemID] += quantity;
        }
        else
        {
            items[itemID] = quantity;
        }

        Debug.Log($"Item added: {itemID} x{quantity}");

        // アイテム収集イベントを発火
        GameEvents.ItemCollected(itemID, quantity);
    }

    public int GetItemCount(string itemID)
    {
        return items.ContainsKey(itemID) ? items[itemID] : 0;
    }

    public bool HasItem(string itemID, int quantity = 1)
    {
        return GetItemCount(itemID) >= quantity;
    }
}
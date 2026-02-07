using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObject/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] string itemName;
    public string ItemName => itemName;
    [SerializeField] Sprite icon;
    public Sprite Icon => icon;

}

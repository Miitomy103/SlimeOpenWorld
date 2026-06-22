using UnityEngine;

/// <summary>
/// プレイヤーがインタラクト操作できるオブジェクトの共通インターフェース。
/// </summary>
public interface IInteractable
{
    bool CanInteract { get; }
    string GetInteractText { get; }
    void Interact(GameObject player);
    string InteractableID { get; }
}

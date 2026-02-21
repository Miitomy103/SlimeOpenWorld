using UnityEngine;

[RequireComponent(typeof(IdDataBehaviour))]
public class Chest : MonoBehaviour,IInteractable
{
    [SerializeField]private bool isOpen = false;
    [SerializeField]private ItemData itemData;

    bool canInteract = true;
    public bool CanInteract => canInteract;
    public string GetInteractText => "開く";

    [SerializeField] private string interactableID;
    public string InteractableID => interactableID;

    [SerializeField] Animator animator;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }
    public void Interact(GameObject player)
    {
        Debug.Log("チェストを開けた！");
        animator.SetTrigger("Open");
        canInteract = false;
        GameEvents.ObjectInteracted(interactableID);
    }
    public void OpenChest()
    {
        ItemCanvas.Instance.GetItem(itemData);
    }
}

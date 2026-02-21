using UnityEngine;

public class SceneMoveInteract : MonoBehaviour,IInteractable
{
    public bool CanInteract => true;

    public string GetInteractText => "ƒ{ƒX•”‰®";

    [SerializeField] private string interactableID;
    public string InteractableID => interactableID;

    public void Interact(GameObject player)
    {
        Debug.Log("SceneMoveInteract: Interact");
        SceneController.Instance.LoadScene("BossScene", new Vector3(0, 0, 0));
    }

}

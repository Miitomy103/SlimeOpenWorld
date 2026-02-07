using UnityEngine;

public class InteractJump : MonoBehaviour, IInteractable
{
    public bool CanInteract => true;

    public string GetInteractText => "ƒWƒƒƒ“ƒv";

    public void Interact(GameObject player)
    {
        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
        }
    }

}

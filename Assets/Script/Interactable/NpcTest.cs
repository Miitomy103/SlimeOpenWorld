using System;
using System.Collections;
using UnityEngine;

public class NpcTest : MonoBehaviour,IInteractable
{
    [SerializeField]private string npcName;

    [SerializeField] bool canInteract = true;
    public bool CanInteract => canInteract;

    public string GetInteractText=>npcName;

    public void Interact(GameObject player)
    {
        Debug.Log($"Hello! I'm {npcName}");

    }
}

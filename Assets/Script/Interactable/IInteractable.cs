using System;
using System.Collections;
using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    string GetInteractText { get; }
    void Interact(GameObject player);
}

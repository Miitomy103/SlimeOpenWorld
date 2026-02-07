using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICapability
{
    bool IsWait { get; }
    void HandleInput(PlayerInput input);
}

using UnityEngine;

public interface IPossess
{
    string PossessId { get; }
    Transform Transform { get; }
    bool CanPossess { get; }
    HostBase GetHost();

}

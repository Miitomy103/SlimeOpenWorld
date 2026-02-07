using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PossessHuman : PossessBase
{
    [SerializeField] bool canPossess = true;
    public override bool CanPossess => canPossess;
}

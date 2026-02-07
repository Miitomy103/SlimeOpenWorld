using UnityEngine;

public class PossessHorse : PossessBase,IPossess
{
    [SerializeField] bool canPossess = true;
    public override bool CanPossess => canPossess;
}

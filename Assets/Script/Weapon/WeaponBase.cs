using UnityEngine;

/// <summary>
/// ‘ÎÛ‚ÉUŒ‚‚ğ—^‚¦‚éŠî’êƒNƒ‰ƒX
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] int attackPower = 10;
    public int AttackPower => attackPower;
    public abstract void AttackStart(IAttackCoroutine attackCoroutine);
}

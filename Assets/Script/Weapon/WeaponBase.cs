using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] int attackPower = 10;
    public int AttackPower => attackPower;
    public abstract void AttackStart(IAttackCoroutine attackCoroutine);
}

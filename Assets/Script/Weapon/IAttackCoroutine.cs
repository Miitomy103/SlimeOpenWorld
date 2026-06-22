using System;
using Cysharp.Threading.Tasks;

/// <summary>
/// 攻撃のコルーチンを提供するインターフェース
/// </summary>
public interface IAttackCoroutine 
{
    UniTask AttackCoroutineAsync(Action enabled,Action disabled);
}

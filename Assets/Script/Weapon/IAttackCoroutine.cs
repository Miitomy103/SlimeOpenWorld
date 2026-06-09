using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// 攻撃のコルーチンを提供するインターフェース
/// </summary>
public interface IAttackCoroutine 
{
    UniTask AttackCoroutineAsync(Action enabled,Action disabled);
}

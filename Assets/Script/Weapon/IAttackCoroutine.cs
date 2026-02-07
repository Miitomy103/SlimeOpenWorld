using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public interface IAttackCoroutine 
{
    UniTask AttackCoroutineAsync(Action enabled,Action disabled);
}

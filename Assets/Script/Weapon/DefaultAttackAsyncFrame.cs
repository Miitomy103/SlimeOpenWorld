using Cysharp.Threading.Tasks;
using System;

public class DefaultAttackAsyncFrame : IAttackCoroutine
{
    readonly int onEnable;
    readonly int onDisable;
    public async UniTask AttackCoroutineAsync(Action enabled, Action disabled)
    {
        await UniTask.DelayFrame(onEnable);
        enabled?.Invoke();
        await UniTask.DelayFrame(onDisable);
        disabled?.Invoke();
    }

    public DefaultAttackAsyncFrame(int enableFrame,int disableFrame)
    {
        this.onEnable = enableFrame;
        this.onDisable = disableFrame;
    }
}

using Cysharp.Threading.Tasks;
using System;

public class DefaultAttackCoroutine :IAttackCoroutine
{
    private readonly float enableTime;
    private readonly float disableTime;

    public async UniTask AttackCoroutineAsync(Action enabled, Action disabled)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(enableTime));
        enabled?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(disableTime));
        disabled?.Invoke();
    }


    public DefaultAttackCoroutine(float enableTime, float disableTime)
    {
        this.enableTime = enableTime;
        this.disableTime = disableTime;
    }
}

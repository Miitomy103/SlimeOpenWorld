using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AnimAttackAcyncFrame : IAttackCoroutine
{
    readonly float onEnable;
    readonly float onDisable;

    readonly Action enabled;
    readonly Action disabled;
    public async UniTask AttackCoroutineAsync(Action enabled, Action disabled)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(onEnable));
        enabled?.Invoke();
        this.enabled?.Invoke();
        await UniTask.Delay(TimeSpan.FromSeconds(onDisable));
        disabled?.Invoke();
        this.disabled?.Invoke();
    }

    public AnimAttackAcyncFrame(int enable,int disable,int sampleFrame,float animatorSpeed=1,Action enabled=null,Action disabled=null)
    {
        onEnable= enable / ((float)sampleFrame*animatorSpeed);
        onDisable = disable/((float)sampleFrame * animatorSpeed);
        this.enabled = enabled;
        this.disabled = disabled;
    }
    public AnimAttackAcyncFrame(IAnimAttackAcyncFrameData data, Action enabled = null, Action disabled = null)
    {
        onEnable = data.Enable / ((float)data.SampleFrame * data.AnimatorSpeed);
        onDisable = data.Disable / ((float)data.SampleFrame * data.AnimatorSpeed);
        this.enabled = enabled;
        this.disabled = disabled;
    }
}

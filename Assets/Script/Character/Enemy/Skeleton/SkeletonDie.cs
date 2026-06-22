using UnityEngine;
using Cysharp.Threading.Tasks;
using InGame;

/// <summary>
/// スケルトンの死亡ステート。死亡アニメーション再生後にOnDieを呼ぶ
/// </summary>
[System.Serializable]
public class SkeletonDie : IState<Skeleton>
{
    public void DoExit(Skeleton owner)
    {

    }

    public void DoStart(Skeleton owner)
    {
        Debug.Log("Skeleton DieStart");
        owner.animator.CrossFadeInFixedTime("Death", 0.1f);
        Coroutine(owner).Forget();
    }

    async UniTask Coroutine(Skeleton owner)
    {
        await UniTask.WaitForSeconds(2.0f);
        owner.OnDie?.Invoke();

    }

    public void DoUpdate(Skeleton owner)
    {
    }
}

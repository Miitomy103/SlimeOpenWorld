using UnityEngine;
using InGame;
using Cysharp.Threading.Tasks;

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

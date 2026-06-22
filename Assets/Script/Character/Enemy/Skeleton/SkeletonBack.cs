using UnityEngine;
using Cysharp.Threading.Tasks;
using InGame;

/// <summary>
/// スケルトンの後退ステート。プレイヤーから一定距離離れたら戦闘ステートへ戻る
/// </summary>
[System.Serializable]
public class SkeletonBack : IState<Skeleton>
{
    [SerializeField] float backwardSpeed = 3f;
    [SerializeField] float backwardDistance = 3f;
    [SerializeField] float backwardDuration = 1.0f;

    bool isBack = false;
    bool isAwait;
    public void DoExit(Skeleton owner)
    {

    }

    public void DoStart(Skeleton owner)
    {
        isAwait = true;
        isBack = true;
        MoveBack(owner).Forget();
    }
    async UniTask MoveBack(Skeleton owner)
    {
        if(IsDistance(owner))
        {
            isBack = false;
            isAwait = false;
            return;
        }
        await UniTask.WaitForSeconds(1f); // 少し待ってから後退開始
        isAwait = false; 
    }

    public void DoUpdate(Skeleton owner)
    {
        RotationSet(owner);
        if (isBack)
        {
            owner.animator.SetBool("IsMove", true);
            Vector3 backward = -owner.transform.forward;
            owner.agent.Move(backwardSpeed * Time.deltaTime * backward);
            owner.MoveSpeedAnimation();

            if (IsDistance(owner))
            {
                isBack = false;
                owner.animator.SetBool("IsMove", false);
            }
        }

        if (!isAwait&&!isBack)
        {
            owner.stateMachine.ChangeState(owner.SkeletonButtle);
        }
    }
    public void RotationSet(Skeleton owner)
    {
        Transform target = PlayerController.Instance.HostBase.transform;
        // --- 向きをターゲットに向ける ---
        Vector3 direction = (target.position - owner.transform.position).normalized;
        direction.y = 0; // 上下は無視

        if (direction != Vector3.zero)
        {
            // 回転をスムーズに
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    // 遠い=> true
    public bool IsDistance(Skeleton owner)
    {
        Transform host = PlayerController.Instance.HostBase.transform;
        float distance = Vector3.Distance(owner.transform.position, host.position);
        if (distance >= backwardDistance)
        {
            isBack = false;
            return true;
        }
        return false;
    }
}

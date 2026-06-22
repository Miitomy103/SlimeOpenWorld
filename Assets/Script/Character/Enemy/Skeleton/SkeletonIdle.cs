using UnityEngine;
using InGame;

[System.Serializable]
public class SkeletonIdle : IState<Skeleton>
{
    public EnemyPattern pattern;

    bool isReturn = false;
    public void DoExit(Skeleton owner)
    {
    }

    public void DoStart(Skeleton owner)
    {
        isReturn = (Vector3.Distance(owner.transform.position, owner.defaultPosition) >= 0.5f);
    }

    public void DoUpdate(Skeleton owner)
    {
        if(isReturn)Return(owner);
        else owner.animator.SetBool("IsMove", false);

    }

    private void Return(Skeleton owner)
    {
        if(Vector3.Distance(owner.transform.position,owner.defaultPosition)>=0.5f)
        {
            if (owner.agent.isOnNavMesh && owner.agent.isActiveAndEnabled)
            {

                // プレイヤーに向かって移動
                owner.agent.isStopped = false;
            owner.agent.SetDestination(owner.defaultPosition);
            }
        }
        else
        {
            owner.agent.isStopped = true;
            isReturn = false;
        }
    }
}

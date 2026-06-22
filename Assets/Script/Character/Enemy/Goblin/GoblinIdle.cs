using UnityEngine;
using InGame;

/// <summary>
/// ゴブリンの待機ステート。設定されたIdleDataに応じて静止/徘徊を行う
/// </summary>
public class GoblinIdle : IState<Goblin>
{
    [SerializeField] IdleData[] idleDatas;
    int stateCount;

    IdleData currentIdleData;

    [SerializeField] Transform[] transforms;

    bool typeStart;
    public void DoExit(Goblin owner)
    {

    }

    public void DoStart(Goblin owner)
    {
    }

    public void DoUpdate(Goblin owner)
    {
        switch(currentIdleData.idleType)
        {
            case IdleType.Stay:
                Stay(owner);
                break;
            case IdleType.WalkAround:
                WalkAround(owner);
                break;
        }
    }

    void Stay(Goblin owner)
    {
        owner.animator.SetBool("IsMove", false);
    }
    void WalkAround(Goblin owner)
    {

    }



}
public struct IdleData
{
    public IdleType idleType;
    public float idleTime;
}
public enum IdleType
{
    Stay,
    WalkAround
}
using InGame;

/// <summary>
/// 状態の基底クラス
/// </summary>
public abstract class StateBase : IState<HostBase>
{
    public virtual void DoExit(HostBase owner)
    {

    }

    public virtual void DoStart(HostBase owner)
    {

    }

    public virtual void DoUpdate(HostBase owner)
    {

    }
}

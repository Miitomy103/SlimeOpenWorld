using InGame;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateBase : IState<HostBase>
{
    public virtual void DoExit(HostBase owner)
    {
        throw new System.NotImplementedException();
    }

    public virtual void DoStart(HostBase owner)
    {
        throw new System.NotImplementedException();
    }

    public virtual void DoUpdate(HostBase owner)
    {
        throw new System.NotImplementedException();
    }
}

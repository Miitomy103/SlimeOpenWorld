using System;

namespace StateMachine
{
    public interface IStateMachine : IDisposable
    {
        IState CurrentState { get; }
    }
}

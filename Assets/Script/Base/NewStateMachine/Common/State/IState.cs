using UnityEngine;
namespace StateMachine
{

    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }

    public interface IStateKey<TKey>
    {
        TKey Key { get; }
    }

    public interface IState<TKey> : IState, IStateKey<TKey>
    {
        IStateTransition<TKey> Transition { set; }
    }
}

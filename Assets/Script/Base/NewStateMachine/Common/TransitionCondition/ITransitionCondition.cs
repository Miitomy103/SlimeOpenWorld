using UnityEngine;

namespace StateMachine
{
    public interface ITransitionCondition<TKey>
    {
        bool CanTransition();
        void Transition();
    }
}

using UnityEngine;

namespace StateMachine
{
    public interface IStateTransition<TKey>
{
    bool IsContainKey(TKey key);
    void Transition(TKey key);
}
}

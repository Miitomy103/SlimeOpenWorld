using System;
using UnityEngine;

namespace InGame
{
    public class StateMachine<T>
    {
        private IState<T> currentState;
        public IState<T> CurrentState => currentState;
        public T owner { get; private set; }

        public Action<T> OnStateChanged;

        public StateMachine(T owner)
        {
            this.owner = owner;
        }
        public void ChangeState(IState<T> newState)
        {
            if (newState is ICanBool can)
            {
                if (!can.CanBool) return;
            }

            currentState?.DoExit(owner);

            currentState = newState;

            currentState?.DoStart(owner);

            OnStateChanged?.Invoke(owner);
        }
        public void Update()
        {
            if (currentState != null)
            {
                currentState.DoUpdate(owner);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// ステートマシン本体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class StateMachine<TKey> : IStateMachine, IStateTransition<TKey>
    {
        private Dictionary<TKey, IState<TKey>> m_states;

        private IState<TKey> m_currentState;


        /// <summary>
        /// 現在のステート。
        /// </summary>
        public IState CurrentState => m_currentState;


        /// <summary>
        /// ステート一覧を登録し、各ステートの遷移先としてこのインスタンスを設定する。
        /// </summary>
        /// <param name="states"></param>
        /// <param name="comparer"></param>
        public void RegisterState(IEnumerable<IState<TKey>> states, IEqualityComparer<TKey> comparer = null)
        {
            m_states = new Dictionary<TKey, IState<TKey>>(comparer);

            foreach (var state in states)
            {
                m_states[state.Key] = state;
                m_states[state.Key].Transition = this;
            }
        }


        /// <summary>
        /// 現在のステートのOnUpdateを呼び出す。
        /// </summary>
        public void Update()
        {
            m_currentState?.OnUpdate();
        }


        /// <summary>
        /// 指定したキーのステートが登録済みか判定する。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsContainKey(TKey key)
        {
            return m_states?.ContainsKey(key) ?? false;
        }


        /// <summary>
        /// 指定したキーのステートへ遷移する。
        /// </summary>
        /// <param name="key"></param>
        public void Transition(TKey key)
        {
            if (m_states == null)
            {
                Debug.LogWarning($"ステートが登録されていないです。");
                return;
            }

            if (!m_states.TryGetValue(key, out var nextState))
            {
                Debug.LogWarning($"{key}に対応するステートが見つかりません。");
                return;
            }

            m_currentState?.OnExit();
            m_currentState = nextState;
            m_currentState.OnEnter();

        }


        /// <summary>
        /// ステートを1件追加登録する。
        /// </summary>
        /// <param name="state"></param>
        public bool AddState(IState<TKey> state)
        {
            if (state == null)
            {
                Debug.LogWarning($"ステートの追加が失敗しました。ステートがNullです。");
                return false;
            }

            if (m_states.TryAdd(state.Key, state))
            {
                m_states[state.Key].Transition = this;
                return true;
            }
            else
            {
                Debug.LogWarning($"ステートの追加が失敗しました。キーが既に存在します。");
                return false;
            }
        }


        /// <summary>
        /// 登録済みのステートをすべて解放する。
        /// </summary>
        public void Dispose()
        {
            m_states = null;
            m_currentState = null;
        }
    }
}

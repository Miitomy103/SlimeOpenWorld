namespace StateMachine
{
    /// <summary>
    /// 遷移条件の基底クラス。CanTransitionがtrueを返すと、指定したキーのステートへ遷移する。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class TransitionConditionBase<TKey> : ITransitionCondition<TKey>
    {
        private readonly IStateTransition<TKey> m_transition;

        private readonly TKey m_key;


        /// <summary>
        /// 遷移先のステートマシンと遷移先キーを指定して初期化する。
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="key"></param>
        protected TransitionConditionBase(IStateTransition<TKey> transition, TKey key)
        {
            m_transition = transition;
            m_key = key;
        }


        /// <summary>
        /// 登録した遷移先キーへ実際に遷移する。
        /// </summary>
        public void Transition()
        {
            m_transition.Transition(m_key);
        }


        /// <summary>
        /// 遷移条件を満たしているか判定する。
        /// </summary>
        /// <returns></returns>
        public abstract bool CanTransition();
    }
}

namespace StateMachine
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class TransitionConditionBase<TKey> : ITransitionCondition<TKey>
    {
        private readonly IStateTransition<TKey> m_transition;

        private readonly TKey m_key;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="key"></param>
        protected TransitionConditionBase(IStateTransition<TKey> transition, TKey key)
        {
            m_transition = transition;
            m_key = key;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Transition()
        {
            m_transition.Transition(m_key);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool CanTransition();
    }
}

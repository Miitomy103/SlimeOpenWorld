using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// ステートの基底クラス。コンポーネントにセットして使用します。
    /// </summary>
    public abstract class StateBase<TKey> : MonoBehaviour, IState<TKey>
    {
        /// <summary>
        /// 遷移条件のリスト
        /// </summary>
        private List<ITransitionCondition<TKey>> m_conditionList;


        /// <summary>
        /// 遷移先を決定するステートマシン本体への参照。
        /// </summary>
        protected IStateTransition<TKey> transition;


        /// <summary>
        /// IState&lt;TKey&gt;.Transitionの実装。transitionフィールドに代入する。
        /// </summary>
        IStateTransition<TKey> IState<TKey>.Transition { set => transition = value; }


        /// <summary>
        /// このステートを識別するキー。
        /// </summary>
        public abstract TKey Key { get; }


        /// <summary>
        /// 他のステートに遷移するための条件のリストを作ってください。
        /// ステートの存在を確かめるためにtransitionからIsContainKey関数の使用を推奨します。
        /// </summary>
        /// <returns></returns>
        protected abstract List<ITransitionCondition<TKey>> CreateCondition();


        /// <summary>
        /// 初期化処理を実行する。遷移条件を作るため、StateMachineに登録した後呼び出してください。
        /// </summary>
        public void InitializeState()
        {
            m_conditionList = CreateCondition();

            Initialize();

            enabled = false;
        }


        /// <summary>
        /// 初期化処理を実行する。
        /// </summary>
        protected virtual void Initialize() { }


        /// <summary>
        /// 遷移条件をチェックします。
        /// </summary>
        protected void CheckCondition()
        {
            if (m_conditionList == null) { return; }

            foreach (var condition in m_conditionList)
            {
                if (condition.CanTransition())
                {
                    condition.Transition();
                    break;
                }
            }
        }




        /// <summary>
        /// このステートに遷移され始めるタイミングで呼び出されます。
        /// </summary>
        public void OnEnter()
        {
            enabled = true;
            OnStateEnter();
        }


        /// <summary>
        /// このステートから抜け出すタイミングで呼び出されます。
        /// </summary>
        public void OnExit()
        {
            enabled = false;
            OnStateExit();
        }


        /// <summary>
        /// このステートが有効な間、毎フレーム呼び出されます。
        /// </summary>
        public void OnUpdate()
        {
            OnStateUpdate();
        }




        /// <summary>
        /// このステートに遷移され始めるタイミングで呼び出されます。
        /// </summary>
        public virtual void OnStateEnter() { }


        /// <summary>
        /// このステートから抜け出すタイミングで呼び出されます。
        /// </summary>
        public virtual void OnStateExit() { }


        /// <summary>
        /// MonoBehaviourのUpdate関数で呼び出されます。
        /// base.OnStateUpdate()にはCheckCondition()が呼び出されます。
        /// </summary>
        public virtual void OnStateUpdate() => CheckCondition();


        /// <summary>
        /// base.Update()にはOnStateUpdate()が呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            OnUpdate();
        }
    }
}

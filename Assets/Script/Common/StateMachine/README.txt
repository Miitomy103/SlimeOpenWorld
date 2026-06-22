汎用ステートマシンフレームワーク(namespace StateMachine)です。

Assets/Script/OldStateMachine/ にある旧ステートマシンの後継ですが、
OldStateMachineは現在も一部キャラクターで使用中のため削除せず残しています。
新しく実装する場合はこちら(Common/StateMachine)を使ってください。

構成:
- State/             : IState, StateBase<TKey>(コンポーネントにセットして使う基底クラス)
- StateTransition/   : IStateMachine, IStateTransition<TKey>, StateMachine<TKey>(本体)
- TransitionCondition/ : ITransitionCondition<TKey>, TransitionConditionBase<TKey>(遷移条件の基底クラス)
- StateDictionary/   : Inspector上でアタッチ済みのステートを検索・表示するエディタ拡張

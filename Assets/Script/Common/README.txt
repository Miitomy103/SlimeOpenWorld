このフォルダには、他のUnityプロジェクトに移植してもそのまま使える汎用スクリプトを置いています。
Quest, Host, Slime などプロジェクト固有の型に依存するコードはここに置かないでください。

主な内容:
- ObjectPool/ : 汎用オブジェクトプール(ObjectPool<T>, PoolObject, IPoolObject)。使い方はObjectPool/README.txt参照
- OverLap/    : Physics.OverlapSphere/OverlapCapsuleのラッパー(OverlapBase, OverLapSphere, OverLapCapsule)
- StateMachine/ : 汎用ステートマシンフレームワーク。詳細はStateMachine/README.txt参照
- Editor/     : エディタ拡張(InterfaceChanger, TodoList)
- その他      : Rotator, Move, Movement, GizmosUtility, LookAtObject, OnTriggerAction など、
                単体で動く汎用コンポーネント

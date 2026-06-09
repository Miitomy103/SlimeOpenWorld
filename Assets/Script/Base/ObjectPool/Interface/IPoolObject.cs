using System;

/// <summary>
/// このインターフェースとObjectクラスをを実装することで、オブジェクトプールで管理できるようになります。
/// </summary>
public interface IPoolObject
    {
        event Func<IPoolObject, bool> DoReturnToPool;
        void DoEnable();
        void DoDisable();
    }
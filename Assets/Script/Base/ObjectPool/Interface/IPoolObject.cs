using System;

    public interface IPoolObject
    {
        event Func<IPoolObject, bool> DoReturnToPool;
        void DoEnable();
        void DoDisable();
    }
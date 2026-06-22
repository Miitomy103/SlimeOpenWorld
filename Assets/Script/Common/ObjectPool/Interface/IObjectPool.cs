using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// オブジェクトプールの操作を定義するインターフェース。
    /// </summary>
    public interface IObjectPool<T> where T : Object, IPoolObject
    {
        public IReadOnlyList<T> EnableObjects { get; }

        T EnableToPoolObject();
        bool ReturnToPoolObject(T obj);
        void ReturnToAllEnableObjects();
    }
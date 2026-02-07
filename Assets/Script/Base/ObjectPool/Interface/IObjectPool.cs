using System.Collections.Generic;
using UnityEngine;

    public interface IObjectPool<T> where T : Object, IPoolObject
    {
        public IReadOnlyList<T> EnableObjects { get; }

        T EnableToPoolObject();
        bool ReturnToPoolObject(T obj);
        void ReturnToAllEnableObjects();
    }
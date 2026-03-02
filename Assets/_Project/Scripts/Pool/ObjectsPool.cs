using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Pool
{
    public class ObjectsPool<TPoolableObject> where TPoolableObject : MonoBehaviour, IPoolable
    {
        private readonly Transform _enableParentTransform;
        private readonly Transform _disableParentTransform;
        private readonly TPoolableObject _prefab;
        private readonly Stack<TPoolableObject> _disableObjects = new Stack<TPoolableObject>();

        public ObjectsPool(Transform enableParentTransform, Transform disableParentTransform, TPoolableObject prefab)
        {
            _enableParentTransform = enableParentTransform;
            _disableParentTransform = disableParentTransform;
            _prefab = prefab;
        }

        public void Return(TPoolableObject poolableObject)
        {
            poolableObject.gameObject.SetActive(false);
            poolableObject.Release();
            poolableObject.transform.SetParent(_disableParentTransform);
            _disableObjects.Push(poolableObject);
        }

        public TPoolableObject Get(Action<TPoolableObject> getAction = null)
        {
            var poolableObject = _disableObjects.Count > 0 ? _disableObjects.Pop() : Create();
            
            poolableObject.transform.SetParent(_enableParentTransform);
            getAction?.Invoke(poolableObject);
            poolableObject.Recycle();
            poolableObject.gameObject.SetActive(true);
            
            return poolableObject;
        }

        private TPoolableObject Create()
        {
            var poolableObject = Object.Instantiate(_prefab, _disableParentTransform);
            poolableObject.gameObject.SetActive(false);
            return poolableObject;
        }
    }
}
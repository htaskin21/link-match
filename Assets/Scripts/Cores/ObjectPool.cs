using System.Collections.Generic;
using UnityEngine;

namespace Cores
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        protected T _prefab;

        private int _poolSize;
        private Stack<T> _pooledObjects;

        public void CreatePool(int poolSize)
        {
            _poolSize = poolSize;
            _pooledObjects = new Stack<T>(_poolSize);

            for (var i = 0; i < _poolSize; i++)
            {
                var newObject = CreateObject();
                _pooledObjects.Push(newObject);
            }
        }

        public T GetObject()
        {
            return _pooledObjects.Count > 0 ? _pooledObjects.Pop() : CreateObject();
        }

        public void ReturnToPool(T returnedObject)
        {
            returnedObject.gameObject.SetActive(false);
            _pooledObjects.Push(returnedObject);
        }

        private T CreateObject()
        {
            var newObject = Instantiate(_prefab, transform);
            newObject.gameObject.SetActive(false);
            return newObject;
        }
    }
}
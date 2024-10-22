using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class Pool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _poolSize = 10;
        private readonly Queue<T> objects = new();

        private void Awake()
        {
            PreloadPoolObjects();
        }

        void PreloadPoolObjects()
        {
            for (var i = 0; i < _poolSize; i++)
            {
                var poolObject = Instantiate(this._prefab, this._container);
                poolObject.gameObject.SetActive(false);
                this.objects.Enqueue(poolObject);
            }
        }

        public T Rent()
        {
            if (!this.objects.TryDequeue(out T obj))
                obj = Instantiate(_prefab, _container);
            
            obj.gameObject.SetActive(true);
            this.OnSpawned(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (this.objects.Contains(obj))
                return;
            
            obj.gameObject.SetActive(false);
            this.OnDespawned(obj);
            this.objects.Enqueue(obj);
        }

        protected virtual void OnSpawned(T obj)
        {
        }

        protected virtual void OnDespawned(T bullet)
        {
        }
    }
}
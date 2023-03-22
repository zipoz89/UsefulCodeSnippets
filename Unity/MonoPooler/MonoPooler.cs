using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game
{
    public class MonoPooler<T> : MonoBehaviour where T : MonoPoolable
    {
        [SerializeField] private GameObject objectPrefab;

        [SerializeField] private int initialPoolSize = 1;
        [SerializeField] private int maxPoolSize = 10;

        public int CurrentPoolSize { get; private set; } = 0;

        private Queue<T> pool = new Queue<T>();
        private List<T> activeObjects = new List<T>();

        public List<T> ActiveObjects => activeObjects;
        

        public void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                Generate();
            }
        }

        public void DeinitializePool()
        {
            List<T> all = pool.ToList();
            all = all.Concat(activeObjects).ToList();

            foreach (var t in all)
            {
                t.OnDestroyed();
                DestroyObject(t);
            }
        }

        public T Get()
        {
            T obj = TryTake();

            if (obj == null && CurrentPoolSize < maxPoolSize)
            {
                Generate();

                obj = TryTake();
            }

            if (obj != null)
            {
                obj.OnSpawned();
            }

            return obj;
        }

        private T TryTake()
        {
            T result = default(T);

            if (pool.Count > 0)
            {
                result = pool.Dequeue();
                activeObjects.Add(result);
            }

            return result;
        }

        public void Return(T obj)
        {
            activeObjects.Remove(obj);
            obj.OnReturned();
            pool.Enqueue(obj);
        }

        public void ReturnAll()
        {
            for (int i = activeObjects.Count-1; i >= 0 ; i--)
            {
                var o = activeObjects[i];
                activeObjects.Remove(o);
                pool.Enqueue(o);
            }
        }

        private void Generate()
        {
            CurrentPoolSize++;
            GameObject o = Instantiate(objectPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            o.name = objectPrefab.name + CurrentPoolSize;
            o.transform.parent = this.transform;
            
            var pollableScript = o.GetComponent<T>();
            pollableScript.OnGenerated();
            pool.Enqueue(pollableScript);
        }

    }
}
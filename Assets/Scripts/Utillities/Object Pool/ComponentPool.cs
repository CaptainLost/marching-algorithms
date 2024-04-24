using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CptLost.ObjectPool
{
    public class ComponentPool<T> where T : Component
    {
        public HashSet<T> Pool { get; private set; } = new HashSet<T>();

        private T m_templateObject;
        private Transform m_objectParent;

        public ComponentPool(T templateObject, Transform objectParent = null)
        {
            m_templateObject = templateObject;
            m_objectParent = objectParent;
        }

        public void EnqueueObject(T obj)
        {
            obj.gameObject.SetActive(false);

            Pool.Add(obj);
        }

        public void Reset()
        {
            foreach (T obj in Pool)
            {
                EnqueueObject(obj);
            }
        }

        public T DequeueObject()
        {
            T obj = Pool.FirstOrDefault(obj => !obj.gameObject.activeInHierarchy);

            if (obj == null)
            {
                obj = EnqueNewInstance();
            }

            obj.gameObject.SetActive(true);

            return obj;
        }

        public T EnqueNewInstance()
        {
            T obj = Object.Instantiate(m_templateObject, m_objectParent);
            obj.gameObject.SetActive(false);

            Pool.Add(obj);

            return obj;
        }
    }
}

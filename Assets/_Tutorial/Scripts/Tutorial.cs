using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Tutorial
{
    [Serializable]
    public abstract class Tutorial
    {
        [SerializeField] private string name;
        
        
        public UnityEvent onBegin;
        public UnityEvent onComplete;


        public void Begin()
        {
            OnBegin();
            onBegin?.Invoke();
        }

        
        protected void Complete()
        {
            OnComplete();
            onComplete?.Invoke();
        }

        protected virtual void OnBegin()
        {
            
        }

        protected virtual void OnComplete()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }
    }
}
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace General
{
    public static class LazyCoroutines
    {
        private const string BehaviourObjectName = "[Lazy Coroutine Runner]";


        private static LazyCoroutineBehaviour Behaviour
        {
            get
            {
                if (!ms_Behaviour)
                {
                    ms_Behaviour = new GameObject(BehaviourObjectName)
                        .AddComponent<LazyCoroutineBehaviour>();
                    
                    Object.DontDestroyOnLoad(ms_Behaviour);
                }

                return ms_Behaviour;
            }
        }
        
        
        private static LazyCoroutineBehaviour ms_Behaviour;
        
        
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return Behaviour.StartCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine == null) return;
            
            Behaviour.StopCoroutine(coroutine);
        }

        public static void StopAllCoroutines()
        {
            Behaviour.StopAllCoroutines();
        }

        public static Coroutine WaitForFrame(Action action)
        {
            return StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                yield return null;
                action?.Invoke();
            }
        }

        public static Coroutine WaitForFrames(int count, Action action)
        {
            return StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                for (var i = 0; i < count; i++)
                {
                    yield return null;
                }
                
                action?.Invoke();
            }
        }
        
        public static Coroutine WaitForSeconds(float delay, Action action)
        {
            return StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                var startTime = Time.time;

                while (Time.time - startTime < delay) yield return null;
                
                action?.Invoke();
            }
        }
        
        public static Coroutine WaitForSecondsRealtime(float delay, Action action)
        {
            return StartCoroutine(Routine());
            
            
            IEnumerator Routine()
            {
                var startTime = Time.unscaledTime;

                while (Time.unscaledTime - startTime < delay) yield return null;
                
                action?.Invoke();
            }
        }

        public static Coroutine EverySeconds(Func<float> secondsGetter, Action action)
        {
            return StartCoroutine(Routine());
            

            IEnumerator Routine()
            {
                var startTime = Time.time;
                var interval = secondsGetter.Invoke();
                
                while (true)
                {
                    if (Time.time - startTime < interval)
                    {
                        yield return null;
                        continue;
                    }

                    startTime = Time.time;
                    interval = secondsGetter.Invoke();
                    
                    action?.Invoke();

                    yield return null;
                }
            }
        }

        public static Coroutine While(Func<bool> condition, Action action)
        {
            return StartCoroutine(Routine());


            IEnumerator Routine()
            {
                while (condition.Invoke())
                {
                    action?.Invoke();
                    yield return null;
                }
            }
        }
        
        public static Coroutine Until(Func<bool> condition, Action action)
        {
            return StartCoroutine(Routine());


            IEnumerator Routine()
            {
                while (!condition.Invoke())
                {
                    action?.Invoke();
                    yield return null;
                }
            }
        }
        
        
        
        
        private class LazyCoroutineBehaviour : MonoBehaviour
        {
            
        }
    }
}
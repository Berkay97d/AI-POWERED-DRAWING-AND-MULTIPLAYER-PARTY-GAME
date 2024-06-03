using System;
using System.Collections;
using General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Compatetion.Baseball.Scripts
{
    public class FruitManager : MonoBehaviour
    {
        public static FruitManager Instance;
        
        [SerializeField] private Fruit[] fruitPrefabArray;
        [SerializeField] private Transform[] slicesFruitPrefabArray;
        [SerializeField] private Transform[] fruitCreateTransformArray;
        [SerializeField] private float createDelayTime;
        [SerializeField] private float destroyDelayTime;
        [SerializeField] private AnimationCurve fallCurve;

        public event Action OnFruitsCreated;

        private int m_FruitIndex;
        private int m_CurrentRound;
        

        private void Awake()
        {
            Instance = this;
        }


        public void CreateFruit()
        {
            StartCoroutine(CreateFruitCoroutine(createDelayTime, destroyDelayTime));
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CreateFruitCoroutine(float createDelay, float destroyDelay)
        {
            while (m_CurrentRound < GameManager.Instance.GetRound())
            {
                var randomFruit = GetRandomFruit();
    
                foreach (var fruitCreateTransform in fruitCreateTransformArray)
                {
                    Fruit fruit = Instantiate(randomFruit, fruitCreateTransform);
                    fruit.SetFallCurve(fallCurve);
                    
                    fruit.DestroySelf(destroyDelay);
                }

                OnFruitsCreated?.Invoke();
                m_CurrentRound++;

                if (m_CurrentRound == GameManager.Instance.GetRound())
                {
                    LazyCoroutines.WaitForSeconds(2.5f, () =>
                    {
                        GameManager.Instance.GameOver();
                    });
                }
                
                yield return new WaitForSeconds(createDelay);
            }
            // ReSharper disable once IteratorNeverReturns
        }



        private Fruit GetRandomFruit()
        {
            var randomIndex = Random.Range(0, fruitPrefabArray.Length);
            m_FruitIndex = randomIndex;
            
            return fruitPrefabArray[randomIndex];
        }
        
        
        public int GetFruitIndex()
        {
            return m_FruitIndex;
        }


        public Transform[] GetSlicesFruitPrefabArray()
        {
            return slicesFruitPrefabArray;
        }


        public int GetCurrentRound()
        {
            return m_CurrentRound;
        }
    }
}
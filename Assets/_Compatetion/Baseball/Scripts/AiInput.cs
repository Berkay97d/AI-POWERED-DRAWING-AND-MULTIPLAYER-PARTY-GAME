using System;
using _Tutorial;
using General;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Compatetion.Baseball.Scripts
{
    public class AiInput : MonoBehaviour, IContesterInput
    {
        [SerializeField] private AnimationCurve[] accuracyCurveArray;
        [SerializeField] private AnimationCurve tutorialCurve;
        
        public event Action OnKickInput;

        private int m_RandomCurve;
        private bool m_IsTutorial = true;
        

        private void Start()
        {
            if (TutorialManager.IsTutorialFinished())
            {
                FruitManager.Instance.OnFruitsCreated += OnFruitsCreated;
            }
            else
            {
                FruitManager.Instance.OnFruitsCreated += CalculateTutorialCurve;
            }

            
            m_RandomCurve = Random.Range(0, accuracyCurveArray.Length);
            
            
        }
        

        private void OnFruitsCreated()
        {
            // 0.68f - 0.9f  0.79f
            
            var t = Random.Range(0f, 1f);

            t = accuracyCurveArray[m_RandomCurve].Evaluate(t);
            
            var time = Mathf.Lerp(0.68f, 0.90f, t);
            
            
            
            LazyCoroutines.WaitForSeconds(time, () =>
            {
                OnKickInput?.Invoke();
            });
        }


        private void CalculateTutorialCurve()
        {
            var t = Random.Range(0f, 1f);

            t = tutorialCurve.Evaluate(t);
            
            var time = Mathf.Lerp(0.68f, 0.90f, t);
            
            
            
            LazyCoroutines.WaitForSeconds(0.4f, () =>
            {
                OnKickInput?.Invoke();
            });
        }


        private void OnDestroy()
        {
            FruitManager.Instance.OnFruitsCreated -= OnFruitsCreated;
            FruitManager.Instance.OnFruitsCreated -= CalculateTutorialCurve;
        }
    }
}
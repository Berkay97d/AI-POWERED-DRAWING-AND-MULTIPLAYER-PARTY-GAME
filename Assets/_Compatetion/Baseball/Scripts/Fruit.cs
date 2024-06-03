using General;
using UnityEngine;

namespace _Compatetion.Baseball.Scripts
{
    public class Fruit : MonoBehaviour
    {
        private AnimationCurve m_FallCurve;
        private float m_StartY;
        private float m_EndY;
        private float m_FallProgress;
        private float m_DestroyDelayTime;


        private void Start()
        {
            m_StartY = transform.position.y;
            m_EndY = 0f;
        }


        private void Update()
        {
            Fall();
        }

        
        private void Fall()
        {
            m_FallProgress += Time.deltaTime * (1f / m_DestroyDelayTime);

            var time = m_FallCurve.Evaluate(m_FallProgress);
            var y = Mathf.Lerp(m_StartY, m_EndY, time);
            var position = transform.position;
            position.y = y;

            transform.position = position;
        }


        public void DestroySelf(float destroyDelay)
        {
            m_DestroyDelayTime = destroyDelay;
            Destroy(gameObject, destroyDelay);
        }


        public void SetFallCurve(AnimationCurve fallCurve)
        {
            m_FallCurve = fallCurve;
        }
    }
}
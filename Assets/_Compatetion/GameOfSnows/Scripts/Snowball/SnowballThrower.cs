using System;
using System.Collections;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using _Compatetion.GameOfSnows.Scripts.Provider;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowballThrower : MonoBehaviour
    {
        [SerializeField] private float throwSpeed;
        [SerializeField] private SnowballCollision snowballCollision;
        
        public event EventHandler OnSnowballThrowed; 

        private InputProvider m_InputProvider;
        private Vector3 m_TargetPosition;
        private float m_LerpTime = 0f;
        private bool m_IsThrowed = false;
        private ContesterMovement m_ContesterMovement;


        private void Awake()
        {
            m_ContesterMovement = GetComponentInParent<ContesterMovement>();
            m_InputProvider = GetComponentInParent<InputProvider>();
        }


        private void Update()
        {
            if (m_InputProvider.GetHorizontal() != 0)
            {
                m_TargetPosition.x = m_InputProvider.GetHorizontal();
            }
            if (m_InputProvider.GetVertical() != 0)
            {
                m_TargetPosition.z = m_InputProvider.GetVertical();
            }


            if ((!Input.GetButtonUp("Fire1") || m_IsThrowed ||
                 m_ContesterMovement.GetInputProvider() is not PlayerInputProvider) ||
                snowballCollision.transform.localScale.y < 1.5f) return;
            Throw();
        }


        public void TryThrow()
        {
            if (m_IsThrowed) return;
            
            Throw();
        }
        
        
        public void Throw()
        {
            m_IsThrowed = true;

            // snowballCollision.transform.DORotate(Vector3.forward,10f).SetSpeedBased();
            OnSnowballThrowed?.Invoke(this, EventArgs.Empty);
            
            transform.parent = new RectTransform();
             
            Vector3 targetPosition = transform.position + (m_TargetPosition * throwSpeed);
            
            m_ContesterMovement.SetSnowballCollision(null);
            m_ContesterMovement.GetContesterCollision().SetSnowballCollusion(null);
            SetContesterMovement(null);
            
            StartCoroutine(MoveObject(targetPosition, 1));
            
            Destroy(gameObject, 1.1f);
        }
        

        private IEnumerator MoveObject(Vector3 targetPosition, float duration)
        {
            float time = 0f;
            Vector3 startPosition = transform.position;

            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

                time += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }


        public ContesterMovement GetContesterMovement()
        {
            return m_ContesterMovement;
        }


        public void SetContesterMovement(ContesterMovement contesterMovement)
        {
            m_ContesterMovement = contesterMovement;
        }


        public Vector3 GetSnowballDirection()
        {
            return m_TargetPosition;
        }
    }
}
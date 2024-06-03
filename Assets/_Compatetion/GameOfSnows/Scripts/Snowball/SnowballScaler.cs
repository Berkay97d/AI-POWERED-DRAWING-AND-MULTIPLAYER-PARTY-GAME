using System;
using _Compatetion.GameOfSnows.Ozer.Scripts;
using UnityEngine;

namespace _Compatetion.GameOfSnows.Scripts.Snowball
{
    public class SnowballScaler : MonoBehaviour
    {
        [SerializeField] private GameObject snowballVisual;
        [SerializeField] private float maxScale;
        [SerializeField] private float scaleSpeed;

        private SnowballCollision m_SnowballCollision;
        private SnowballThrower m_SnowballThrower;
        private ContesterMovement m_ContesterMovement;
        private bool m_CanScale = true;
        private bool m_IsMax;
        

        private void Awake()
        {
            m_SnowballThrower = GetComponent<SnowballThrower>();
            m_ContesterMovement = GetComponentInParent<ContesterMovement>();
            m_SnowballCollision = GetComponentInChildren<SnowballCollision>();
        }


        private void Start()
        {
            m_SnowballThrower.OnSnowballThrowed += OnSnowballThrowed;
            GameManager.OnGameFinish += OnGameFinish;
        }

        private void OnGameFinish()
        {
            SetCanScale(false);
            
        }

        private void OnSnowballThrowed(object sender, EventArgs e)
        {
            m_CanScale = false;
        }


        private void Update()
        {
            if (Math.Abs(snowballVisual.transform.localScale.y - 8f) < 0.1f)
            {
                m_SnowballThrower.TryThrow();
            }

            if (!m_CanScale) return;
            Scale();
        }

        private void Scale()
        {
            Vector3 velocity = m_ContesterMovement.GetVelocity();
            velocity.y = 0f;

            bool isMoving = velocity.magnitude > 0.65f;
            if (isMoving)
            {
                var deltaScale = m_ContesterMovement.GetVelocity().magnitude * scaleSpeed * Time.deltaTime;

                Vector3 newScale = snowballVisual.transform.localScale + new Vector3(deltaScale, deltaScale, deltaScale);
                newScale = Vector3.ClampMagnitude(newScale, maxScale);

                snowballVisual.transform.localScale = newScale;
            }
        }

        public SnowballCollision GetSnowballCollusion()
        {
            return m_SnowballCollision;
        }


        public float GetScale()
        {
            return transform.localScale.magnitude;
        }


        private void OnDestroy()
        {
            m_SnowballThrower.OnSnowballThrowed -= OnSnowballThrowed;
            GameManager.OnGameFinish -= OnGameFinish;
        }


        public GameObject GetSnowballVisual()
        {
            return snowballVisual;
        }


        public float GetMaxScaleValue()
        {
            return maxScale;
        }


        public void SetCanScale(bool canScale)
        {
            m_CanScale = canScale;
        }
    }
}
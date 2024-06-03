using System;
using _Compatetion.Jumping.Scripts.Input;
using UnityEngine;

namespace _Compatetion.Jumping.Scripts.Contesters
{
    public class ContesterCollisionHandler : MonoBehaviour
    {
        [SerializeField] private Transform centerTransform;
        [SerializeField] private Transform feetTransform;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private LayerMask obstacleControlLayerMask;
        

        private bool m_IsTouchGround;
        private bool m_IsTouchObstacle;
        private bool m_IsTouchObstacleControl;
        private AiInput m_AiInput;


        private void Awake()
        {
            TryGetAiInput();
        }


        private void Update()
        {
            CheckGround();
            CheckObstacle();
            CheckObstacleControl();
        }

        
        private void CheckObstacleControl()
        {
            var isTouchObstacleControl = Physics.CheckBox(centerTransform.position, skinnedMeshRenderer.bounds.extents,
                Quaternion.identity, obstacleControlLayerMask);

            m_IsTouchObstacleControl = isTouchObstacleControl;
        }


        private void CheckObstacle()
        {
            var isTouchObstacle = Physics.CheckBox(centerTransform.position, skinnedMeshRenderer.bounds.extents,
                Quaternion.identity, obstacleLayerMask);

            m_IsTouchObstacle = isTouchObstacle;
        }


        private void TryGetAiInput()
        { 
            transform.parent.parent.TryGetComponent(out AiInput aiInput);

            m_AiInput = aiInput;
        }


        private void CheckGround()
        {
            var isTouchGround = Physics.CheckSphere(feetTransform.position, 0.1f, groundLayerMask);

            m_IsTouchGround = isTouchGround;
        }


        public AiInput GetAiInput()
        {
            return m_AiInput;
        }
        

        public bool GetIsTouchObstacle()
        {
            return m_IsTouchObstacle;
        }
        

        public bool GetIsTouchGround()
        {
            return m_IsTouchGround;
        }


        public bool GetIsTouchObstacleControl()
        {
            return m_IsTouchObstacleControl;
        }
    }
}
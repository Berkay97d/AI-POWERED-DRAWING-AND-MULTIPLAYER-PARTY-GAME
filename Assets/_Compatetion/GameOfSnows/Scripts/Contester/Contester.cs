using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Compatetion.GameOfSnows.Ozer.Scripts
{
    public class Contester : MonoBehaviour
    {
        [SerializeField] private Image deadIcon;
        [SerializeField] private Image kingIcon;
        [SerializeField] private Transform snowballTransformParent;
        [SerializeField] private Transform zoomPoint;
        
        
        private ContesterMovement m_ContesterMovement;
        private int m_Point;
        private int m_Index;
        private bool m_IsDead;
        private Vector3 m_FirstPos;


        private void Awake()
        {
            m_ContesterMovement = GetComponent<ContesterMovement>();
        }


        private void Start()
        {
            m_FirstPos = transform.position;
        }


        public int GetIndex()
        {
            return m_Index;
        }

        public void SetIndex(int index)
        {
            m_Index = index;
        }
        
        public void AddPoint(int point)
        {
            m_Point = m_Point + point;
        }


        public int GetPoint()
        {
            return m_Point;
        }


        public void SetIsDead(bool isDead)
        {
            m_IsDead = isDead;
        }


        public bool IsDead()
        {
            return m_IsDead;
        }


        public void SetActiveDeadIcon(bool b)
        {
            deadIcon.gameObject.SetActive(b);
        }


        public void SetActiveKingIcon(bool b)
        {
            kingIcon.gameObject.SetActive(b);
        }


        public Vector3 GetFirstPosition()
        {
            return m_FirstPos;
        }


        public ContesterMovement GetContesterMovement()
        {
            return m_ContesterMovement;
        }


        public Transform GetSnowballTransformParent()
        {
            return snowballTransformParent;
        }


        public void DestroySnowball()
        {
            if (snowballTransformParent.childCount > 0)
            {
                Destroy(snowballTransformParent.GetChild(0).gameObject);
            }
        }

        public Transform GetZoomPoint()
        {
            return zoomPoint;
        }
    }
}
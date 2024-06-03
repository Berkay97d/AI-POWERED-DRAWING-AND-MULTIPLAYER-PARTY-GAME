using System;
using UnityEngine;

namespace _Compatetion.Baseball.Scripts
{
    public class Contester : MonoBehaviour
    {
        [SerializeField] private ContesterType contesterType;
        [SerializeField] private ContesterKicker contesterKicker;

        public event EventHandler<int> OnContesterGetPoint; 

        private int m_Point;
        private int m_Index;


        public int GetIndex()
        {
            return m_Index;
        }
        
        public void SetIndex(int index)
        {
            m_Index = index;
        }

        public void SetContesterPoint(int point)
        {
            m_Point = point;
        }


        public void AddContesterPoint(int point)
        {
            m_Point = m_Point + point;
            OnContesterGetPoint?.Invoke(this, point);
        }


        public int GetPoint()
        {
            return m_Point;
        }


        public ContesterType GetContesterType()
        {
            return contesterType;
        }


        public ContesterKicker GetContesterKicker()
        {
            return contesterKicker;
        }
    }
}
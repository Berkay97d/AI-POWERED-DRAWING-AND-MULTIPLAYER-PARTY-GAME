using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Compatetion.Jumping.Scripts.UI
{
    public class DoubleClickListener : MonoBehaviour, IPointerDownHandler 
    {
        [Tooltip ("Duration between 2 clicks in seconds")]
        [Range (0.01f, 0.5f)] public float doubleClickDuration = 0.4f ;
    
        public event Action OnDoubleClick ;

        private float m_Time;
        private int m_Clicks;


        public void OnPointerDown (PointerEventData eventData) 
        {
            if (m_Clicks <= 0)
            {
                m_Clicks += 1;
                m_Time = Time.time;
                return;
            }

            if (m_Clicks < 1) return;

            var elapsedTime = Time.time - m_Time;
            m_Time = Time.time;

            if (elapsedTime > doubleClickDuration) return;

            OnDoubleClick?.Invoke();
            
            m_Clicks = 0;
        }
    }
}
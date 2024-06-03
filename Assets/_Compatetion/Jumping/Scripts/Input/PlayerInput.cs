using System;
using _Compatetion.Baseball.Scripts;
using _Compatetion.Jumping.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Compatetion.Jumping.Scripts.Input
{
    public class PlayerInput : MonoBehaviour, IInput, IPointerDownHandler
    {
        public event Action OnJump;
        
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (CountdownManager.Instance.GetIsGamePaused()) return;
                OnJump?.Invoke();
        }
    }
}
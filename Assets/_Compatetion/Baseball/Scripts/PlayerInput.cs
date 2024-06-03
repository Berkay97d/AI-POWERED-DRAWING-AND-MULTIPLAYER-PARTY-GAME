using System;
using UnityEngine;

namespace _Compatetion.Baseball.Scripts
{
    public class PlayerInput : MonoBehaviour, IContesterInput
    {
        public event Action OnKickInput;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnKickInput?.Invoke();
            }
        }
    }
}
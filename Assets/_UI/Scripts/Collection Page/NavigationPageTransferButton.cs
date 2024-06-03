using System;
using UnityEngine;

namespace UI
{
    public class NavigationPageTransferButton : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private new Collider collider;
        
        
        public event EventHandler OnClick;

        public RectTransform GetRect()
        {
            return rect;
        }

        public void SetInteractable(bool value)
        {
            collider.enabled = value;
        }
        
        
        private void OnMouseDown()
        {
            OnClick?.Invoke(this,EventArgs.Empty);
        }
    }
}
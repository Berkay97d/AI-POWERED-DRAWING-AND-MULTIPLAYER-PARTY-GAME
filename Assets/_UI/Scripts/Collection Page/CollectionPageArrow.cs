using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CollectionPageArrow : MonoBehaviour
    {
        [SerializeField] private new Collider collider;
        [SerializeField] private bool isBackArrow;

        public event EventHandler<bool> OnCollectionPageArrowClicked;


        public void SetInteractable(bool value)
        {
            collider.enabled = value;
        }
        
        
        private void OnMouseDown()
        {
            RaiseOnCollectionPageArrowClicked(isBackArrow);
        }

        private void RaiseOnCollectionPageArrowClicked(bool i)
        {
            OnCollectionPageArrowClicked?.Invoke(this,i);
        }
    }
}
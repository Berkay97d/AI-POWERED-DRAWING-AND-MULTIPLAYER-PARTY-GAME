using System;
using General;
using Unity.VisualScripting;
using UnityEngine;

namespace DontFall
{
    public class Contester : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform feetTransform;
        
        public event EventHandler<Contester> OnContesterDrop;

        protected bool isDead;
        protected Rigidbody rb;

        private int index;
        private bool isGrounded;


        protected virtual void Update()
        {
            CheckIsGrounded();
        }


        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int _index)
        {
            index = _index;
        }
        

        private void CheckIsGrounded()
        {
            /*var boxPosition = referenceTransform.position - new Vector3(0f, boxSize.y / 2f, 0f);
            isGrounded = Physics.OverlapBox(boxPosition, boxSize / 2f, Quaternion.identity, groundLayer).Length > 0;*/
            var ig = Physics.CheckSphere(feetTransform.position, 0.1f, groundLayer);
            isGrounded = ig;
        }
        
        public bool GetIsGrounded()
        {
            return isGrounded;
        }
        
        public void SetIsDead(bool isD)
        {
            isDead = isD;
        }

        public bool GetIsDead()
        {
            return isDead;
        }

        public void RaiseOnContesterDrop()
        {
            OnContesterDrop?.Invoke(this, this);
        }

        
    }
}
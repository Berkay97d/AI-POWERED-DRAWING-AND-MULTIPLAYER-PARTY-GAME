using UnityEngine;

namespace UI
{
    public class CardSlot : MonoBehaviour
    {
        private bool isFull;


        public void SetIsFull(bool i)
        {
            isFull = i;
        }

        public bool GetIsFull()
        {
            return isFull;
        }
    }
}
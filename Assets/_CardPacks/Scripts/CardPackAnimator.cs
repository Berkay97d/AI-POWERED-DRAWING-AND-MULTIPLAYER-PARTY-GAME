using System;
using UnityEngine;

namespace _CardPacks.Scripts
{
    public class CardPackAnimator : MonoBehaviour
    {
        private static readonly int OpenHash = Animator.StringToHash("open");
        private static readonly int PullHash = Animator.StringToHash("pull");

        
        [SerializeField] private Animator animator;


        public event Action OnOpenCompleted;
        

        public void PlayOpen()
        {
            animator.SetTrigger(OpenHash);
        }

        public void PlayPull()
        {
            animator.SetTrigger(PullHash);
        }
        
        
        private void OnAnimatorOpenCompleted()
        {
            OnOpenCompleted?.Invoke();
        }
    }
}
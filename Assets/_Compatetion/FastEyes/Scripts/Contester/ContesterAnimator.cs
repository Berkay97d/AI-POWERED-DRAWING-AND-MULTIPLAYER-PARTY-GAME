using System;
using UnityEngine;

namespace FastEyes
{
    
    public class ContesterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Contester contester;


        private void Start()
        {
            contester.OnArrowShooted += OnArrowShooted;
        }

        private void OnArrowShooted(object sender, EventArgs e)
        {
            PLayShotAnimation();
        }

        private void PLayShotAnimation()
        {
            animator.SetTrigger("Shoot");
        }
        
    }
}
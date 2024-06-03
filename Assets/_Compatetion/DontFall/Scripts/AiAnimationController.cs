using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFall
{

    public class AiAnimationController : MonoBehaviour
    {
        [SerializeField] private AI ai;
        [SerializeField] private Animator animator;

        private AnimationState animationState;
        
        private void Start()
        {
            animationState = AnimationState.Idle;
        }

        private void Update()
        {
            if (ai.GetVelocity().magnitude > .1f )
            {
                TriggerRun();
                return;
            }
            
            TriggerIdle();
        }

        private void TriggerRun()
        {
            if (animationState == AnimationState.Run) return;
            
            animator.SetTrigger("StartRun");
            animationState = AnimationState.Run;
        }

        private void TriggerIdle()
        {
            if (animationState == AnimationState.Idle) return;
            
            animator.SetTrigger("StartIdle");
            
            animationState = AnimationState.Idle;
        }

        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontFall
{

    public enum AnimationState
    {
        Idle,
        Run
    }
    
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator animator;

        private AnimationState animationState;


        private void Start()
        {
            animationState = AnimationState.Idle;
        }

        private void Update()
        {
            if (playerController.GetMoveDirection() != Vector3.zero )
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
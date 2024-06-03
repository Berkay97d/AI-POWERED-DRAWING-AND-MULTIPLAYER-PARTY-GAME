using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MakeItLonger
{

    public class ContesterAnimCont : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private bool isPlayer;


        private void Start()
        {
            animator.SetBool("isPlayer", isPlayer);
        }
    }
}
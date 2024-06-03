using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackJack
{ 
    public class BetController : MonoBehaviour
    {
        [SerializeField] private BetAreaController betAreaController;
        
        public static BetController Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameController.Instance.OnGameStarted += OnGameStarted;
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            Currency.Instance.DecreaseCurrency(betAreaController.GetBetValue());
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace BlackJack
{

    public class BetAreaController : MonoBehaviour
    {
        [Foldout("Buttons")] [SerializeField] private Button IncreaseBetButton;
        [Foldout("Buttons")] [SerializeField] private Button DecreaseBetButton;
        [SerializeField] private TMP_Text betValueText;
        
        private int betValue;
        
        
        private void Start()
        {
            IncreaseBetButton.onClick.AddListener(IncreaseBet);
            DecreaseBetButton.onClick.AddListener(DecreaseBet);
        }

        private void Update()
        {
            betValueText.text = betValue.ToString();
        }

        private void IncreaseBet()
        {
            var newBet = GetCurrentBet() + 5;
            if (newBet <= Currency.Instance.GetCurrenct())
            {
                SetCurrentBet(newBet);
            }
        }

        private void DecreaseBet()
        {
            if (GetCurrentBet() <= 0) return;

            var newBet = GetCurrentBet() - 5;
            SetCurrentBet(newBet);
        }
        
        private void SetCurrentBet(int value)
        {
            betValue = value;
        }

        private int GetCurrentBet()
        {
            return betValue;
        }

        public int GetBetValue()
        {
            return betValue;
        }
    }
}
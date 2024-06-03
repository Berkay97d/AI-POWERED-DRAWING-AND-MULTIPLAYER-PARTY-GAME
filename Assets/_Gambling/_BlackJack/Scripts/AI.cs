using System;
using UnityEngine;

namespace BlackJack
{
    public class AI : Gambler
    {
        
        protected override void Start()
        {
            base.Start();
            
            OnAiTurnStarted += OnOnAiTurnStarted;
        }

        private void OnOnAiTurnStarted(object sender, EventArgs e)
        {
            gamblerCards[1].TurnFrontAnimaton();
        }

        private void Update()
        {
            
        }
    }
}
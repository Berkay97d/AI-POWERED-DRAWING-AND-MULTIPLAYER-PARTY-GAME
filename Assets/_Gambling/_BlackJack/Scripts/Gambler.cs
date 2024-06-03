using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace BlackJack
{
    public abstract class Gambler : MonoBehaviour
    {
        [SerializeField] private TMP_Text openHandPointText;
        [SerializeField] private float cardGap;
        
        public event EventHandler OnGamblerTakeCard;
        public event EventHandler OnGamblerStop;

        public event EventHandler OnAiTurnStarted;

        protected readonly List<Card> gamblerCards = new List<Card>();
        private int openHandPoint;
        
        
        protected virtual void Start()
        {
            OnGamblerTakeCard += OnTakeCard;
        }

        private void OnTakeCard(object sender, EventArgs e)
        {
            UpdateScoreText();

            CheckPlayerState();
        }

        private void CheckPlayerState()
        {
            if (IsHandBomed())
            {
                GameController.Instance.RaiseOnGameOver();
            }

            if (IsHandBlackjack())
            {
                if (this is Player)
                {
                    Debug.Log("AI TURN");
                    OnAiTurnStarted?.Invoke(this, EventArgs.Empty);
                    return;
                }
                GameController.Instance.RaiseOnGameOver();
            }
        }

        private void UpdateScoreText()
        {
            openHandPoint = 0;
            foreach (var card in gamblerCards)
            {
                if (card.GetIsFront())
                {
                    openHandPoint += card.GetValue();
                }
            }

            openHandPointText.text = openHandPoint.ToString();
        }
        
        protected void GetCard()
        {
            var a = GameController.Instance.GiveCard(this, true);
            RaiseOnGamblerTakeCard();
        }
        

        protected void Stop()
        {
            RaiseOnGamblerStop();
        }
        
        public void AddCardToHand(Card card)
        {
            gamblerCards.Add(card);
        }

        public Vector3 GetCardPosition()
        {
            var pos = new Vector3(transform.position.x + gamblerCards.Count * cardGap,
                transform.position.y ,
                transform.position.z - 0.01f * gamblerCards.Count);

            return pos;
        }

        public bool IsHandBlackjack()
        {
            return openHandPoint == 21;
        }

        public bool IsHandBomed()
        {
            return openHandPoint > 21;
        }
        
        public void RaiseOnGamblerTakeCard()
        {
            OnGamblerTakeCard?.Invoke(this, EventArgs.Empty);
        }
        
        private void RaiseOnGamblerStop()
        {
            OnGamblerStop?.Invoke(this, EventArgs.Empty);
        }

        public int GetOpenHandPoint()
        {
            return openHandPoint;
        }

    }
}
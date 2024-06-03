using System;
using UI;
using UnityEngine;

namespace _Tutorial
{
    [Serializable]
    public class LookAtCardTutorial : Tutorial
    {
        [SerializeField] private CraftPageCardInitilazor craftPage;
        [SerializeField] private CompetationBettingCardInitilazor bettingPage;
        [SerializeField] private Vector2 padding;
        [SerializeField] private float pixelPerUnitMultiplier = 1f;
        [SerializeField] private Vector2 handOffset;
        [SerializeField] private float handScale = 1f;
        [SerializeField] private float handAngle;
        [SerializeField] private int cardIndex;


        private Card m_Card;


        protected override void OnBegin()
        {
            if (craftPage)
            {
                m_Card = craftPage.GetCard(cardIndex);
            }
            
            else if (bettingPage)
            {
                m_Card = bettingPage.GetCard(cardIndex);
            }
            
            m_Card.OnCardClicked += OnCardClicked;

            var target = m_Card.GetNormalRect();
            
            Highlighter.HandFocus(target, handOffset, handScale, handAngle);
            Highlighter.Focus(target, padding, pixelPerUnitMultiplier);
        }

        protected override void OnComplete()
        {
            m_Card.OnCardClicked -= OnCardClicked;
        }


        private void OnCardClicked(object sender, Card e)
        {
            Complete();
        }
    }
}